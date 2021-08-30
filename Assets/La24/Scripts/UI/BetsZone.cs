using System.Collections.Generic;
using System.Collections;
using IndieLevelStudio.BetsModule.Controllers;
using IndieLevelStudio.BlackJack.Settings;
using IndieLevelStudio.BoardModule.Controllers;
using IndieLevelStudio.BoardModule.UX;
using IndieLevelStudio.Common;
using IndieLevelStudio.Networking;
using IndieLevelStudio.Networking.Models;
using UnityEngine;
using UnityEngine.UI;
using IndieLevelStudio.Ruleta.Gameplay;

public class BetsZone : UIZone
{
	public override ZoneName Zone {
		get { return ZoneName.Bets; }
	}

	[SerializeField]
	public BetsMoneyController betsMoney;

	[SerializeField]
	private HintManager hints;

	[SerializeField]
	private BoardController genericBoard;

	[SerializeField]
	private MoneyHandler economyHandler;

	[SerializeField]
	private MoneyStadistics stadistics;

	[SerializeField]
	private MachineSetup machineSetup;

	[SerializeField]
	private Button btnStadistics;

	[SerializeField]
	public Button btnCashOut;

	[SerializeField]
	private Text moneyText;

	[SerializeField]
	private Text betText;

	[SerializeField]
	private Text gainText;

	[SerializeField]
	private RawImage imgLastGna;

	[SerializeField]
	private GameSettings settings;

	[SerializeField]
	private GameObject moneyHeader;

	[SerializeField]
	private GameObject blocker;

	private bool cameFromGameplay;
	private bool isPlayerMakingBets;
	public bool openBets;

	private void Awake ()
	{
		btnStadistics.onClick.AddListener (ShowStadistics);
		btnCashOut.onClick.AddListener (CashOut);

		betsMoney.OnDenominationTypeChanged += OnDenominationTypeChanged;
		betsMoney.OnBetsFinished += GoingToGameplay;
		genericBoard.OnBetsOpened += BetsOpened;

		economyHandler.OnMoneyValueChanged += MoneyHandler_OnMoneyValueChanged;
		economyHandler.OnMoneyFromZero += ShowHints;
		economyHandler.OnUserBetValueChanged += EconomyHandler_OnUserBetValueChanged;
		economyHandler.OnGainValueChanged += EconomyHandler_OnGainValueChanged;

		isPlayerMakingBets = true;
		economyHandler.Gain = 0;
		economyHandler.UserBet = 0;

		betsMoney.OnPlayerStartBets += BetsMoney_OnPlayerStartBets;
	}

	void BetsMoney_OnPlayerStartBets ()
	{
		isPlayerMakingBets = true;
	}

	private void BetsOpened(bool betsOpened)
	{
		openBets = betsOpened;
		btnStadistics.interactable = !betsOpened;
		btnCashOut.interactable = !betsOpened;
	}

	private void Start ()
	{
		moneyHeader.SetActive (true);
		betsMoney.gameObject.SetActive (true);

		ShowHints ();
	}

	private void ShowHints ()
	{
		hints.ShowHints (XMLManager.UserMessages);
	}

	private void OnEnable ()
	{
		LoadLastGNA ();
		btnStadistics.interactable = true;

		if (cameFromGameplay) {
			blocker.SetActive (true);
			ModulesGlobalObjects.UserBet = 0;

			GameplayManager.Instance.GetBalance (() => {
				economyHandler.UserMoney = ModulesGlobalObjects.UserMoney;
				economyHandler.UserBet = ModulesGlobalObjects.UserBet;
				economyHandler.Gain = ModulesGlobalObjects.UserGain;

				StartCoroutine (ScreenshotRecorder.SaveScreenshot (XMLManager.ScreenshotsDirectory, 0, ClearGameBoard));
				genericBoard.buttonReplayLastBet.interactable = true;
			});
		}
	}

	private void ClearGameBoard ()
	{
		float delay = 0;
		betsMoney.ClearBoardAnimated (out delay);

		LeanTween.delayedCall (gameObject, delay, () => blocker.SetActive (false));
	}

	public void ClearAllBets ()
	{
		if (betsMoney.IsBetsOpened)
			betsMoney.ClearBets ();
	}

	private void CashOut ()
	{
		GameplayManager.Instance.CashOut ();

		/*
		if (economyHandler.UserMoney <= 0)
			return;

		GenericTransaction<CashOutRequest> request = new GenericTransaction<CashOutRequest> ();
		request.msgName = "cashOut";
		request.msgDescrip = new CashOutRequest ();

		request.msgDescrip.deno = betsMoney.denoms.CurrentDenomination;

		string json = JsonUtility.ToJson (request);
		WebServiceManager.Instance.SendJsonData<GenericTransaction<CashOutResponse>> (XMLManager.UrlGeneric, json, "authorization", GlobalObjects.BackendToken, CashOutResponse, TransactionFailed);
		*/
	}

	private void CashOutResponse (GenericTransaction<CashOutResponse> response)
	{
		GlobalObjects.SaveTransactionAttributes (response);
		economyHandler.UserMoney = 0;
	}

	private void ShowStadistics ()
	{
		GenericTransaction<StadisticsRequest> request = new GenericTransaction<StadisticsRequest> ();
		request.msgName = "getStat";
		request.msgDescrip = new StadisticsRequest ();

		string json = JsonUtility.ToJson (request);
		WebServiceManager.Instance.SendJsonData<GenericTransaction<StadisticsResponse>> (XMLManager.UrlGeneric, json, "authorization", GlobalObjects.BackendToken, GetStatsResponse, TransactionFailed);
	}

	private void GetStatsResponse (GenericTransaction<StadisticsResponse> response)
	{
		GlobalObjects.SaveTransactionAttributes (response);
		stadistics.Show (response.msgDescrip.last, response.msgDescrip.hot, response.msgDescrip.cold);
	}

	private void LoadLastGNA ()
	{
		if (!string.IsNullOrEmpty (GameplayManager.Instance.LastLCDNumber))
			imgLastGna.texture = Resources.Load ("LCD_Display/" + GameplayManager.Instance.LastLCDNumber) as Texture;
	}

	private void EconomyHandler_OnUserBetValueChanged (string obj)
	{
		if (isPlayerMakingBets)
			betText.text = obj;
		else {
			Dictionary<Bet, int> betsDictionary = ModulesGlobalObjects.LastBetRegister;
			int betValue = 0;

			foreach (KeyValuePair<Bet, int> bet in betsDictionary)
				betValue += bet.Value  * ModulesGlobalObjects.CurrentDenomination;

			if (ModulesGlobalObjects.IsMoney)
				betText.text = "$" + betValue.ToString ("N0");
			else
				betText.text = ((betValue/ ModulesGlobalObjects.CurrentDenomination).ToString ());
		}
	}

	private void MoneyHandler_OnMoneyValueChanged (string money)
	{
		moneyText.text = money;
	}

	private void EconomyHandler_OnGainValueChanged (string gain)
	{
		gainText.text = gain;
	}

	private void GoingToGameplay (Dictionary<Bet, int> userBets)
	{
		GameplayManager.Instance.ActiveBlocker ();
		
		if (GlobalObjects.IsRecoveryMode) {
			isPlayerMakingBets = false;
			BetResponse recoveryBetResponse = new BetResponse ();

			recoveryBetResponse.balance = GlobalObjects.RecoveryInfo.balance;
			recoveryBetResponse.bonusNonrestricted = GlobalObjects.RecoveryInfo.bonusNonrestricted;
			recoveryBetResponse.bonusRestricted = GlobalObjects.RecoveryInfo.bonusRestricted;
			recoveryBetResponse.canMiniGame = GlobalObjects.RecoveryInfo.canMiniGame;
			recoveryBetResponse.gain = GlobalObjects.RecoveryInfo.gain;
			recoveryBetResponse.deno = GlobalObjects.RecoveryInfo.userDeno;
			recoveryBetResponse.gnaResult = GlobalObjects.RecoveryInfo.gnaResult;
			recoveryBetResponse.idPlaysession = GlobalObjects.RecoveryInfo.idPlaysession;
			recoveryBetResponse.game = new List<BetResultData> ();

			foreach (BetRecoveryData r in GlobalObjects.RecoveryInfo.game)
				recoveryBetResponse.game.Add ((BetResultData)r);

			StartCoroutine (AfterRecoveryBetResponse (recoveryBetResponse));

			return;
		}


		GenericTransaction<BetRequest> request = new GenericTransaction<BetRequest> ();
		request.msgName = "bet";
		request.msgDescrip = new BetRequest ();

		request.msgDescrip.deno = betsMoney.denoms.CurrentDenomination;
		request.msgDescrip.bets = CreateBetsData (userBets);

		isPlayerMakingBets = false;

		string json = JsonUtility.ToJson (request);
		
		WebServiceManager.Instance.SendJsonData<GenericTransaction<BetResponse>> (XMLManager.UrlGeneric, json, "authorization", GlobalObjects.BackendToken, BetResponse, TransactionFailed);
	}

	private List<BetGameData> CreateBetsData (Dictionary<Bet, int> userBets)
	{
		List<BetGameData> data = new List<BetGameData> ();
		BetGameData currentData;
		foreach (KeyValuePair<Bet, int> pair in userBets) {
			currentData = new BetGameData ();
			currentData.id = GetIndexOf (pair.Key.transform, pair.Key.transform.parent);
			currentData.amount = pair.Value;
			currentData.value = GetRewardValue (pair.Key);
			currentData.type = pair.Key.RewardTypeText;

			data.Add (currentData);
		}
		return data;
	}

	private string GetRewardValue (Bet bet)
	{
		if (!string.IsNullOrEmpty (bet.DataValue))
			return bet.DataValue;

		string val = string.Empty;
		foreach (string s in bet.numbers)
			val += s + "_";

		val = val.Remove (val.Length - 1);
		return val;
	}

	private int GetIndexOf (Transform son, Transform parent)
	{
		int i = 0;
		for (i = 0; i < parent.childCount; i++) {
			if (parent.GetChild (i) == son)
				break;
		}
		return i;
	}

	private void BetResponse (GenericTransaction<BetResponse> response)
	{
		StartCoroutine (AfterBetResponse (response));
	}

	private IEnumerator AfterBetResponse (GenericTransaction<BetResponse> response)
	{
		yield return StartCoroutine (ScreenshotRecorder.SaveScreenshot (XMLManager.ScreenshotsDirectory, 0));

		yield return new WaitForEndOfFrame ();

		machineSetup.Hide ();
		GameplayManager.Instance.SaveBetsData (response.msgDescrip);
		GlobalObjects.idPlaysession = response.msgDescrip.idPlaysession;

		StartGameplay ();
	}

	private IEnumerator AfterRecoveryBetResponse (BetResponse recoveryResponse)
	{
		yield return StartCoroutine (ScreenshotRecorder.SaveScreenshot (XMLManager.ScreenshotsDirectory, 0));

		yield return new WaitForEndOfFrame ();

		machineSetup.Hide ();
		GameplayManager.Instance.SaveBetsData (recoveryResponse);
		GlobalObjects.idPlaysession = recoveryResponse.idPlaysession;

		StartGameplay ();
	}

	private void StartGameplay ()
	{
		UIController.Instance.DisableAll ();
		GameplayManager.Instance.StartGameplay ();

		btnStadistics.interactable = false;
		cameFromGameplay = true;
	}

	public void OnDenominationTypeChanged (DenomType type)
	{
		economyHandler.UserMoney = ModulesGlobalObjects.UserMoney;
		economyHandler.UserBet = ModulesGlobalObjects.UserBet;
		economyHandler.Gain = ModulesGlobalObjects.UserGain;
	}

	private void TransactionFailed (string errorMessage)
	{
		Alert.Instance.Show ("Transaction Failed", errorMessage);
	}

	public void RecoveryMode ()
	{
		List<Bet> recoveryBets = new List<Bet> ();
		List<Bet> bets = genericBoard.Bets;

		Debug.Log ("Recovery info: bets: " + GlobalObjects.RecoveryInfo.game.Count);

		foreach (BetRecoveryData b in GlobalObjects.RecoveryInfo.game) {
			Debug.Log (b.amount + " //// " + b.type);

			for (int i = 0; i < bets.Count; i++) {
				if (b.value == GetRewardValue (bets [i]) && b.type == bets [i].RewardTypeText) {
					recoveryBets.Add (bets [i]);

					for (int j = 0; j < b.amount; j++)
						genericBoard.OnClickBet (bets [i]);

					break;
				}
			}
		}

		PlaySound.audios.PlayFX ("Button Sound 3");

		LeanTween.delayedCall (2, betsMoney.PlayImmediatly);
	}
}