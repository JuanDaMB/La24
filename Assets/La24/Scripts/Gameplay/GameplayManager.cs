using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IndieLevelStudio.BetsModule.Controllers;
using IndieLevelStudio.BlackJack.Settings;
using IndieLevelStudio.BoardModule.Controllers;
using IndieLevelStudio.BoardModule.UX;
using IndieLevelStudio.Common;
using IndieLevelStudio.Networking;
using IndieLevelStudio.Networking.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using IndieLevelStudio.Ruleta.Gameplay;
using IndieLevelStudio.IES.SlotMinigames;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
	[SerializeField]
	private CashoutAlert CashoutAlertText;
		
	[SerializeField] 
	private GameObject CashoutContainer;

	[SerializeField]
	private Button yes, no;
	
	public static GameplayManager Instance;

	public event Action<int> OnButtonPressed;

	private Action onGetBalanceSucceded;

	[SerializeField]
	private HintManager hints;

	[SerializeField]
	private MoneyHandler economy;

	[SerializeField]
	private BetsZone betsZone;

	[SerializeField]
	private FinalResult finalResult;

	[Header ("UI")]

	[SerializeField]
	public SocketGame socket;

	[SerializeField]
	private DenominationsController denoms;

	[SerializeField]
	private GameObject blockerButtons;

	[Header ("Gameplay")]
	[SerializeField]

	public GameSettings settings;
	public LoadLauncher launcher;

	public Animator animatorCamera;
	public Bola ball;
	public MeshRenderer display;
	
	[HideInInspector]
	public List<Bet> ChosenBets;

	private string numberSelected;

	public string LastLCDNumber {
		set { PlayerPrefs.SetString ("LAST_LCD_NUMBER", value); }
		get { return PlayerPrefs.GetString ("LAST_LCD_NUMBER", string.Empty); }
	}

	public bool AlreadyEnded;
	public bool PlayOnce;

	public int totalGamesToRestart;
	private int PlayedGames;

	private void Awake ()
	{
		PlayedGames = 0;
		Instance = this;
		GlobalObjects.IsCashOutMode = false;

		#if !UNITY_WEBGL && !UNITY_EDITOR
		Cursor.visible = false;
		#endif
	}

	private void Start ()
	{
		yes.onClick.AddListener(ContinueCashOut);
		no.onClick.AddListener(EndCashout);
		
	}

	private void ConfigureSocket ()
	{
#if !UNITY_WEBGL
		socket.OnSocketTextReceived += Socket_OnSocketTextReceived;
		if (XMLManager.SocketBlock)
			socket.SocketConnectionFail += SocketBlock;

		if (!string.IsNullOrEmpty (XMLManager.SocketURL))
			socket.InitializeSocket (XMLManager.SocketURL);
#endif
	}

	private void Socket_OnSocketTextReceived (string message)
	{
		if (message == "RESTART") {
			SceneManager.LoadScene (0);
			return;
		}

		if (CashoutContainer.activeInHierarchy)
		{
			switch (message)
			{
				case "BTN3":
					if (yes.interactable)
					{
						ConfirmCashout();
					}
					break;
				case "BTN7":
					if (no.interactable)
					{
						EndCashout();
					}
					break;
			}
			return;
		}
		
		if (GlobalObjects.IsCashOutMode || GlobalObjects.IsRecoveryMode)
			return;

		switch (message) {
		case "NEWBALANCE":
			GetBalance ();
			break;

		case "BTN1":
			if (betsZone.btnCashOut.interactable)
			{
				CashOut();
			}
			break;

		case "BTN2":
			TriggerPhysicButtonPressed (2);
			break;

		case "BTN3":
			TriggerPhysicButtonPressed (3);
			break;

		case "BTN4":
			TriggerPhysicButtonPressed (4);
			break;

		case "BTN5":
			//TriggerPhysicButtonPressed (5);
			break;

		case "BTN6":
			TriggerPhysicButtonPressed (6);
			break;

		case "BTN7":
			TriggerPhysicButtonPressed (7);
			break;

		case "BTN8":
			TriggerPhysicButtonPressed (8);
			break;

		case "BTN9":
			TriggerPhysicButtonPressed (9);
			break;

		case "BTN10":
			TriggerPhysicButtonPressed (10);
			break;
		}
	}

	private void TriggerPhysicButtonPressed (int button)
	{
		if (OnButtonPressed != null)
			OnButtonPressed (button);
	}

	public void CashOut()
	{
		if (!betsZone.betsMoney.IsBetsOpened && ModulesGlobalObjects.UserBet <= 0)
		{
			if (economy.UserMoney <= 0)
			{
				EnterCashoutMode (false);
				if (socket.SocketRunning) {
					socket.SendString("UNLOCK");
				}
				return;
			}

			CashoutContainer.SetActive(true);
			GetBalance();
			EnterCashoutMode (true);

			if (socket.SocketRunning)
			{
				socket.SendString("BTN1");
			}
		}
	}
	
	private void ContinueCashOut()
	{
		CashoutContainer.SetActive(false);
		ConfirmCashout();
	}

	private void EndCashout()
	{
		CashoutContainer.SetActive(false);
			
		EnterCashoutMode (false);
		
		if (socket.SocketRunning) {
			socket.SendString("UNLOCK");
		}
	}

	private void ConfirmCashout()
	{
		GenericTransaction<CashOutRequest> request = new GenericTransaction<CashOutRequest> ();
		request.msgName = "cashOut";
		request.msgDescrip = new CashOutRequest ();

		request.msgDescrip.deno = denoms.CurrentDenomination;

		EnterCashoutMode (true);
		if (socket.SocketRunning) {
			Debug.Log ("Sending cashout");
			socket.SendString ("CASHOUT");
		}

		string json = JsonUtility.ToJson (request);
		WebServiceManager.Instance.SendJsonData<GenericTransaction<CashOutResponse>> (XMLManager.UrlGeneric, json, "authorization", GlobalObjects.BackendToken, CashOutResponse, TransactionFailed);
	}

	private void CashOutResponse (GenericTransaction<CashOutResponse> response)
	{
		GlobalObjects.SaveTransactionAttributes (response);
		EnterCashoutMode (true);
	}

	public void EnterCashoutMode (bool cashout)
	{
		GlobalObjects.IsCashOutMode = cashout;
		blockerButtons.SetActive (GlobalObjects.IsCashOutMode);

		Debug.Log ("Cash out mode: " + GlobalObjects.IsCashOutMode);
	}

	public void GetBalance (Action onGetBalanceSucceded = null)
	{
		this.onGetBalanceSucceded = onGetBalanceSucceded;

		GenericTransaction<BalanceRequest> request = new GenericTransaction<BalanceRequest> ();
		request.msgName = "getBalance";
		request.msgDescrip = new BalanceRequest ();

		string json = JsonUtility.ToJson (request);

		WebServiceManager.Instance.SendJsonData<GenericTransaction<BalanceResponse>> (XMLManager.UrlGeneric, json, "authorization", GlobalObjects.BackendToken, GetBalanceSucceded, TransactionFailed);
	}

	private void GetBalanceSucceded (GenericTransaction<BalanceResponse> response)
	{
		GlobalObjects.SaveTransactionAttributes (response);

		betsZone.ClearAllBets ();

		ModulesGlobalObjects.Currency = response.msgDescrip.currency;
		int oldMoney = economy.UserMoney;
		economy.UserMoney = response.msgDescrip.balance + response.msgDescrip.bonusNonrestricted + response.msgDescrip.bonusRestricted;

		CashoutAlertText.SetTextValue(response.msgDescrip.balance + response.msgDescrip.bonusNonrestricted,response.msgDescrip.bonusRestricted);
		
		TryShowHint (economy.UserMoney - oldMoney);

		if (onGetBalanceSucceded != null)
			onGetBalanceSucceded ();

		
	}

	private void TryShowHint (int amount)
	{
		if (amount <= 0)
			return;

		string tx = "Recarga por: $" + amount.ToString ("N0");
		hints.ShowHints (new List<string> () { tx });
	}

	public void SetupGame ()
	{
		ball.OnCompleteMove += TerminarRecorrido;
		totalGamesToRestart = GlobalObjects.TotalGamesToRestart;
		hints.Configure (XMLManager.HintsTime);
		ConfigureSocket ();
	}

	private bool HasRecibedDataFromLauncher ()
	{
		string[] args = System.Environment.GetCommandLineArgs ();
		string input = string.Empty;
		for (int i = 0; i < args.Length; i++) {
			Debug.Log ("ARG " + i + ": " + args [i]);
			if (args [i] == "-folderInput") {
				input = args [i + 1];
			}
		}

		if (!string.IsNullOrEmpty (input)) {
			GlobalObjects.UserMessage = JsonUtility.FromJson<Message> (input);
			if (GlobalObjects.UserMessage != null) {
				Debug.Log ("Los datos han sido recibidos exitosamente");
				return true;
			}
		}
		return false;
	}

	public void StartGameplay ()
	{
		InitializeGameplay ();
	}

	private void TerminarRecorrido()
	{
		blockerButtons.SetActive (false);
		numberSelected = ball.ballName;
		LastLCDNumber = numberSelected;

		display.material.mainTexture = Resources.Load ("LCD_Display/" + LastLCDNumber) as Texture;

		if (!AlreadyEnded)
		{
			//TODO RotarCamara
			// animatorPivotCamera.enabled = true;
			// animatorPivotCamera.Play ("Rotate");
			PlaySound.audios.PlayMusic ("Lobby", true, 0.15f);
			AlreadyEnded = true;
		}
		Invoke ("ShowResult", 0.5f);
	}

	public void SkipBallCameraRotation ()
	{
		ball.Accelerar();
	}

	private void ShowResult ()
	{
		StartCoroutine (ShowFinalUI());
		
		UIController.Instance.EnableButtonOmit (false);
		//TODO Camara quieta a display
		// animatorPivotCamera.enabled = false;
		animatorCamera.Play ("MostrarLCD");
	}

	private IEnumerator ShowFinalUI ()
	{
		numberSelected = ball.ballName;
		
		NumberColor color = NumberColor.Green;
		if (numberSelected.Contains("Negro"))
			color = NumberColor.Black;
		else if (numberSelected.Contains("Rojo"))
			color = NumberColor.Red;

		numberSelected = numberSelected.Replace("Negro", string.Empty);
		numberSelected = numberSelected.Replace("Rojo", string.Empty);

		yield return new WaitForSeconds(0.25f);
		
		int numberAsInt = int.Parse(numberSelected);
		// if (numberAsInt == 1 || numberAsInt == 24)
			// color = NumberColor.Green;
		
		if (!PlayOnce)
		{
			PlayOnce = true;
			PlaySound.audios.PlayFX(numberSelected);
			PlaySound.audios.PlayFX(color.ToString(), 1, PlaySound.audios.GetFxClipLength(numberSelected));
		}

		yield return new WaitForSeconds(2f);
		AlreadyEnded = false;

		
			UIController.Instance.stadistics.SaveBet(new PreviousBet(numberSelected, color.ToString()));
			UIController.Instance.ShowFinalScreen(ChosenBets, numberAsInt, color);
		
	}

	private IEnumerator SaveScreenshot ()
	{
		yield return new WaitForEndOfFrame ();

		byte[] bytes = ScreenCapture.CaptureScreenshotAsTexture ().EncodeToPNG ();
		string directory = Application.persistentDataPath + "/../ScreenShots";

		if (!Directory.Exists (directory))
			Directory.CreateDirectory (directory);

		File.WriteAllBytes (directory + "/LastGNAScreen.png", bytes);
	}

	public void SaveBetsData (BetResponse response)
	{
		economy.UserMoney = response.balance + response.bonusNonrestricted;

		ball.SetNumber(response.gnaResult);

		float gain = ModulesGlobalObjects.IsMoney ? response.gain * denoms.CurrentDenomination : response.gain;
		
		finalResult.SaveAttributes (gain, response.canMiniGame);
	}
	private void SelectStartPoint ()
	{
//TODO elegir donde comienza la camara
	}

	private void InitializeGameplay ()
	{
		if (GlobalObjects.IsRecoveryMode)
			GlobalObjects.IsRecoveryMode = false;
		
		SelectStartPoint ();
		animatorCamera.Play ("StartSorteo"); //TODO Camara iddle

		UIController.Instance.EnableButtonOmit (true);
		blockerButtons.SetActive (false);
		PlayOnce = false;
		//TODO comenzar aqui
		ball.EstablecerMovimiento();

		PlaySound.audios.PlayMusic ("Tension Ball", true, 0.25f);
	}

	public void RestartGame ()
	{
		PlayedGames++;
		if (PlayedGames > totalGamesToRestart)
		{
			blockerButtons.SetActive(true);
			Invoke("Restart",2f);
		}
		animatorCamera.Play ("MostrarLCD");
		UIController.Instance.MoveToZone (ZoneName.Bets);
		PlaySound.audios.PlayMusic ("Lobby", true, 0.15f);
	}

	private void Restart()
	{
		SceneManager.LoadScene(0);
	}

	private void TransactionFailed (string errorMessage)
	{
		Alert.Instance.Show ("Login Failed", errorMessage);
	}

	private void SocketBlock (string socketError)
	{
		Alert.Instance.Show ("Problema con el socket", socketError);
	}

	private void OnDestroy ()
	{
		Instance = null;
	}

	public void StartRecoveryMode ()
	{
		blockerButtons.SetActive (true);
		betsZone.RecoveryMode ();
	}

	public void ActiveBlocker ()
	{
		blockerButtons.SetActive (true);
	}
}