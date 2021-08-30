using System.Collections;
using System.Collections.Generic;
using IndieLevelStudio.BlackJack.Settings;
using IndieLevelStudio.BoardModule.UX;
using IndieLevelStudio.Common;
using IndieLevelStudio.IES.SlotMinigames;
using IndieLevelStudio.IES.SlotMinigames.HighCard;
using IndieLevelStudio.Networking;
using IndieLevelStudio.Networking.Models;
using UnityEngine;
using UnityEngine.UI;

public class FinalResult : UIZone
{
	public override ZoneName Zone {
		get { return ZoneName.Results; }
	}

	private GameplayManager GM {
		get { return GameplayManager.Instance; }
	}

	[SerializeField]
	private Sprite iconWin;

	[SerializeField]
	private Sprite iconLose;

	[SerializeField]
	private Image image;

	[SerializeField]
	private Button btnReload;

	[SerializeField]
	private DuplicatePopup duplicatePopup;

	[SerializeField]
	private MinigameManager minigame;

	private MinigameManager currentMinigame;

	[SerializeField]
	private Transform minigameParent;

	[SerializeField]
	private GameSettings settings;

	[SerializeField]
	private Text total;

	private List<Bet> winnedBets = new List<Bet> ();

	private bool canPlayMinigame;

	private int totalResult;

	private float winnedMoneyValue;

	private string ballTableResult;

	private string userName;
	private string userMail;
	private string userId;

	private bool winSomething;

	private Texture2D screenShot;

	private GameObject TotalContainer {
		get { return total.transform.parent.gameObject; }
	}

	private void Awake ()
	{
		duplicatePopup.OnDuplicate += OnDuplicatePopupResponse;
		btnReload.onClick.AddListener (AddMoneyAndReload);

		duplicatePopup.Setup ();
	}

	private void OnEnable ()
	{
		TotalContainer.SetActive (false);

		if (winSomething) {
			
				PlaySound.audios.PlayMusic ("Winner", true, 0.15f);
				LeanTween.delayedCall (gameObject, 2.5f, ShowMoneyReward);
			
		} else
			Invoke ("ReloadGame", 5);

		StartAnimation ();
	}

	public void SaveAttributes (float winnedValue, bool canPlayMinigame)
	{
		winnedMoneyValue = winnedValue;
		this.canPlayMinigame = canPlayMinigame;
	}

	private void OnDuplicatePopupResponse (bool accept)
	{
		if (accept) {
			currentMinigame = Instantiate (minigame, minigameParent, false);
			currentMinigame.OnFinish += OnFinishDoubleScore;
			currentMinigame.GetComponent<HighCard> ().Gain = winnedMoneyValue;
			currentMinigame.Show ();
		} else
			AddMoneyAndReload ();

		duplicatePopup.Hide ();
	}

	private void OnFinishDoubleScore (bool hasDuplicated, bool canDuplicate)
	{
		if (currentMinigame != null)
			Destroy (currentMinigame.gameObject);

		if (hasDuplicated) {
			winnedMoneyValue *= 2;
			duplicatePopup.Show (winnedMoneyValue, ModulesGlobalObjects.IsMoney);
		} else {
			duplicatePopup.Hide ();

			ModulesGlobalObjects.UserBet = 0;
			winnedMoneyValue = 0;

			LeanTween.delayedCall (gameObject, 3, AddMoneyAndReload);
		}
		UpdateMoneyValue (winnedMoneyValue);
	}

	private void AddMoneyAndReload ()
	{
		ReloadGame ();
	}

	private void StartAnimation ()
	{
		image.rectTransform.localScale = Vector3.zero;
		LeanTween.scale (image.rectTransform, Vector3.one, 0.25f).setOnComplete (() =>
		{
			PlaySound.audios.PlayFX(winSomething ? "FELICITACIONES" : "NO HAS OBTENIDO NADA, INTENTALO DE NUEVO");

			LeanTween.scale (image.rectTransform, Vector3.one * 1.5f, 0.5f).setEase (LeanTweenType.punch);
		});
	}

	private void UpdateMoneyValue (float winned)
	{
		if (ModulesGlobalObjects.IsMoney)
			total.text = "$ " + winned.ToString ("N0");
		else
			total.text = winned.ToString ();
	}

	private void ReloadGame ()
	{
		btnReload.interactable = false;

		ModulesGlobalObjects.UserBet = 0;
		if (ModulesGlobalObjects.IsMoney)
		{
			ModulesGlobalObjects.UserGain = winnedMoneyValue;	
		}
		else
		{
			ModulesGlobalObjects.UserGain = winnedMoneyValue*ModulesGlobalObjects.CurrentDenomination;
		}
		
		GenericTransaction<EndGameRequest> request = new GenericTransaction<EndGameRequest> ();
		request.msgName = "endGame";
		request.msgDescrip = new EndGameRequest ();

		string json = JsonUtility.ToJson (request);
		WebServiceManager.Instance.SendJsonData<GenericTransaction<EndGameResponse>> (XMLManager.UrlGeneric, json, "authorization", GlobalObjects.BackendToken, EndGameResponse, TransactionFailed);
	}

	private void EndGameResponse (GenericTransaction<EndGameResponse> response)
	{
		ModulesGlobalObjects.UserMoney = response.msgDescrip.balance + response.msgDescrip.bonusNonrestricted;

		if (GM.settings.UseLauncher)
			Application.Quit ();
		else
			GameplayManager.Instance.RestartGame ();
	}

	private void ShowMoneyReward ()
	{
		Vector2 centerPos = TotalContainer.transform.position;
		Vector2 upPos = centerPos + (Vector2.up * 10f);

		TotalContainer.transform.position = upPos;
		TotalContainer.SetActive (true);

		UpdateMoneyValue (winnedMoneyValue);

		LeanTween.scale (image.gameObject, Vector3.zero, 0.25f).setOnComplete (() => {
			PlaySound.audios.PlayFX ("HAS OBTENIDO");
			PlaySound.audios.PlayFX (totalResult + "k", 1, PlaySound.audios.GetFxClipLength ("HAS OBTENIDO"));

			LeanTween.moveY (TotalContainer, centerPos.y, 0.5f).setEase (LeanTweenType.easeOutBounce).setOnComplete (ShowDuplicatePopup);
		});
	}

	private void ShowDuplicatePopup ()
	{
		btnReload.gameObject.SetActive (true);

		if (canPlayMinigame)
			duplicatePopup.Show (winnedMoneyValue, ModulesGlobalObjects.IsMoney);
		else
			LeanTween.delayedCall (gameObject, 1, ReloadGame);
	}

	private IEnumerator ShowPromotionalRewards ()
	{
		PlaySound.audios.PlayMusic ("Winner", true, 0.15f);

		yield return new WaitForSeconds (0.75f);

		totalResult = 0;
		foreach (Bet bet in winnedBets)
			totalResult += bet.reward.Data.GetIntValue ();

		Vector2 centerPos = TotalContainer.transform.position;
		Vector2 upPos = centerPos + (Vector2.up * 10f);

		TotalContainer.transform.position = upPos;
		TotalContainer.SetActive (true);

		if (totalResult > 0) {
			total.text = "$ " + totalResult.ToString ("N0");
			SendWinnedBet (total.text, ballTableResult, 1, true);

			LeanTween.scale (image.gameObject, Vector3.zero, 0.25f).setOnComplete (() => {
				PlaySound.audios.PlayFX ("HAS OBTENIDO");
				PlaySound.audios.PlayFX (totalResult + "k", 1, PlaySound.audios.GetFxClipLength ("HAS OBTENIDO"));

				LeanTween.moveY (TotalContainer, centerPos.y, 0.5f).setEase (LeanTweenType.easeOutBounce);
			});
		} else {
			int index = 0;
			string gift = string.Empty;

			LeanTween.scale (image.gameObject, Vector3.zero, 0.25f);

			yield return new WaitForSeconds (0.25f);

			while (index < winnedBets.Count) {
				if (string.IsNullOrEmpty (gift))
					gift = winnedBets [index].reward.Data.value;
				else
					gift += ", " + winnedBets [index].reward.Data.value;

				total.text = winnedBets [index].reward.Data.value;

				PlaySound.audios.PlayFX ("HAS OBTENIDO");
				PlaySound.audios.PlayFX (totalResult + "k", 1, PlaySound.audios.GetFxClipLength ("HAS OBTENIDO"));

				LeanTween.moveY (TotalContainer, centerPos.y, 0.5f).setEase (LeanTweenType.easeOutBounce);

				LeanTween.moveY (TotalContainer, upPos.y, 0.5f).setEase (LeanTweenType.easeInBack);

				index++;
			}
			SendWinnedBet (gift, ballTableResult, index + 1, true);
		}
	}

	private void SendWinnedBet (string bet, string number, int consecutivo, bool reloadAfterComplete = false)
	{
		Debug.Log (bet);
		if (GlobalObjects.IsModeAdmin) {
			Debug.Log ("No se envia la recompensa cuando esta en modo administrador");
			Invoke ("ReloadGame", 5);
			return;
		}
		Invoke ("ReloadGame", 5);
	}

	private void OnPrnitFail (string obj)
	{
		Invoke ("ReloadGame", 2.5f);
	}

	public void ShowResult (List<Bet> bets, string ballResult, NumberColor color)
	{
		ResultNotPromotional ();
	}

	private void ResultNotPromotional ()
	{
		winSomething = winnedMoneyValue > 0;

		if (!winSomething)
			ModulesGlobalObjects.UserBet = 0;

		image.sprite = winSomething ? iconWin : iconLose;
		UIController.Instance.MoveToZone (ZoneName.Results);
	}

	private void TransactionFailed (string errorMessage)
	{
		Alert.Instance.Show ("Login Failed", errorMessage);
	}
}