using System;
using System.Collections;
using System.Collections.Generic;
using IndieLevelStudio.Common;
using IndieLevelStudio.Networking;
using IndieLevelStudio.Networking.Models;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum NumberColor
{
	Red = 1,
	Black = 0,
	Green = 2
}


public class GameManager : MonoBehaviour
{
	public static GameManager Manager;
    [SerializeField] private CoinHandler _coinHandler;
    [SerializeField] private DenominationHandler _denominationHandler;
    [SerializeField] private BetBoard _board;
    [SerializeField] private TextMeshProUGUI maxBetText;
    [SerializeField] public SocketGame socket;
    [SerializeField] private GameObject CashoutContainer;
    [SerializeField] private CashoutAlert CashoutAlertText;
    [SerializeField] private GameObject blockerButtons, canvasUI;
    [SerializeField] private WinScreen winScreen;
    [SerializeField] private Button skipButton;
    [SerializeField] private GameConfig _config;
    [SerializeField] private Estadisticas _estadisticas;
    [SerializeField] private Camera main;
    public CashoutHandler _cashoutHandler;
    public Animator animatorCamera;
    public Bola ball;
    public MeshRenderer display;
    
    public void Start()
    {
	    if (Manager == null)
	    {
		    Manager = this;
	    }
	    else
	    {
		    Destroy(this);
	    }
	    _config.Configuracion(EndBet,DenoChanged,CoinChanged,SetOnBet,CashOut,QuitGame,ShowStatistics,TerminarRecorrido,SkipBallCameraRotation);
    }

    private void QuitGame()
    {
	    socket.SendString("MENU");
	    Debug.Log("Closing game " + GlobalObjects.game);
	    Application.Quit();
    }

    public void ShowStatistics()
    {
	    _estadisticas.StadisticasRequest();
    }

    public void ConfigureGame(float value)
    {
	    _config.BarConfig(value);
	    _estadisticas.SetUp();


#if !UNITY_WEBGL
	    socket.OnSocketTextReceived += Socket_OnSocketTextReceived;
	    if (XMLManager.SocketBlock)
		    socket.SocketConnectionFail += SocketBlock;

	    if (!string.IsNullOrEmpty(XMLManager.SocketURL))
		    socket.InitializeSocket(XMLManager.SocketURL);
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
				    _cashoutHandler.ConfirmCashout();
				    break;
			    case "BTN7":
				    _cashoutHandler.EndCashout();
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
			    if (GlobalObjects.State != GameState.Playing || GlobalObjects.State!= GameState.ShowingResult|| GlobalObjects.State!= GameState.Cashout)
			    {
				    CashOut();   
			    }
			    break;
		    case "BTN5":
			    //TriggerPhysicButtonPressed (5); //TODO revisar el boton
			    break;
	    }
    }
    private void SocketBlock (string socketError)
    {
	    Alert.Instance.Show ("Problema con el socket", socketError);
    }

    public void SetMaxBetValue ()
    {
	    _board.ConfigBoard();
        maxBetText.text = "Apuesta máxima $" + GlobalObjects.MaxBet.ToString ("N0");
    }

    public void CashOut()
	{
		_cashoutHandler.CashOut();
	}

	public void GetBalance (Action onGetBalanceSucceded = null)
	{
		// this.onGetBalanceSucceded = onGetBalanceSucceded;

		GenericTransaction<BalanceRequest> request = new GenericTransaction<BalanceRequest> ();
		request.msgName = "getBalance";
		request.msgDescrip = new BalanceRequest ();

		string json = JsonUtility.ToJson (request);

		WebServiceManager.Instance.SendJsonData<GenericTransaction<BalanceResponse>> (XMLManager.UrlGeneric, json, "authorization", GlobalObjects.BackendToken, GetBalanceSucceded, TransactionFailed);
	}

	private void GetBalanceSucceded (GenericTransaction<BalanceResponse> response)
	{
		GlobalObjects.SaveTransactionAttributes (response);

		// betsZone.ClearAllBets ();
		int oldMoney = GlobalObjects.UserMoneyReal;
		GlobalObjects.UserMoneyReal = response.msgDescrip.balance + response.msgDescrip.bonusNonrestricted + response.msgDescrip.bonusRestricted;

		CashoutAlertText.SetTextValue(response.msgDescrip.balance + response.msgDescrip.bonusNonrestricted,response.msgDescrip.bonusRestricted);
		
		// TryShowHint (economy.UserMoney - oldMoney);

		// if (onGetBalanceSucceded != null)
		// 	onGetBalanceSucceded ();
	}
    
    public void SetCoinAndDeno(List<int> coin, List<int> deno)
    {
        _coinHandler.AssingCoins(coin);
        _denominationHandler.SetArray(deno);
    }

    public void SetOnBet(bool onBet)
    {
        _config.SetOnBet(onBet);
    }

    public void DenoChanged(int value)
    {
        GlobalObjects.Deno = value;
        GlobalObjects.IsMoney = GlobalObjects.IsMoney;
        _coinHandler.ChangeCoinValues(value);
    }

    public void CoinChanged(int value)
    {
        GlobalObjects.Coin = value;
    }

    public void EnableBoard()
    {
	    _board.SetEnabled();
    }
    public void SkipBallCameraRotation ()
    {
	    skipButton.gameObject.SetActive(false);
	    ball.Accelerar();
    }
    private void EndBet()
    {
	    if (!_board.CanBet())
	    {
		    return;
	    }
        GenericTransaction<BetRequest> request = new GenericTransaction<BetRequest> ();
        request.msgName = "bet";
        request.msgDescrip = new BetRequest ();

        request.msgDescrip.deno = GlobalObjects.Deno;
        request.msgDescrip.bets = CreateBetsData (_board.GetBet());

        string json = JsonUtility.ToJson (request);
        WebServiceManager.Instance.SendJsonData<GenericTransaction<BetResponse>> (XMLManager.UrlGeneric, json, "authorization", GlobalObjects.BackendToken, BetResponse, TransactionFailed);
        
    }
    
    private List<BetGameData> CreateBetsData (Dictionary<string, (int value, BetType type)> userBets)
    {
	    List<BetGameData> data = new List<BetGameData> ();
	    foreach (KeyValuePair<string,(int value, BetType type)> pair in userBets)
	    {
		    string key = pair.Key;
		    if (pair.Key.Contains("C"))
		    {
			    key = pair.Key.Replace("C", string.Empty);
		    }

		    if (pair.Key.Contains("F"))
		    {
			    key = pair.Key.Replace("F", string.Empty);
		    }
		    var currentData = new BetGameData
		    {
			    id = 0, amount = pair.Value.value, value = key, type = pair.Value.type.ToString()
		    };

		    data.Add (currentData);
	    }
	    return data;
    }
    
    public void RecoveryGame()
    {
	    AfterBetResponse(GlobalObjects.RecoveryInfo.gnaResult, GlobalObjects.RecoveryInfo.idPlaysession,
		    GlobalObjects.RecoveryInfo.userDeno, GlobalObjects.RecoveryInfo.gain);
    }
    private void BetResponse (GenericTransaction<BetResponse> response)
    {
	    AfterBetResponse(response.msgDescrip.gnaResult, response.msgDescrip.idPlaysession, response.msgDescrip.deno,
		    response.msgDescrip.gain);
    }

    private void AfterBetResponse (int gnaResult, int idPlaysession, int deno, float gain)
    {
	    _board.ClearBoard();
	    main.enabled = true;
	    // yield return StartCoroutine (ScreenshotRecorder.SaveScreenshot (XMLManager.ScreenshotsDirectory, 0));
	    // yield return new WaitForEndOfFrame ();

	    canvasUI.SetActive(false);
	    ball.SetNumber(gnaResult);
	    GlobalObjects.idPlaysession = idPlaysession;
	    GlobalObjects.UserGain = gain * deno;
	    winScreen.SetWinScreen(GlobalObjects.UserGain);

	    StartGameplay();
    }
    
    private void StartGameplay ()
    {
	    if (GlobalObjects.IsRecoveryMode)
		    GlobalObjects.IsRecoveryMode = false;
		
	    skipButton.gameObject.SetActive(true);
	    animatorCamera.Play ("StartSorteo");
	    blockerButtons.SetActive (false);
	    ball.EstablecerMovimiento();

	    PlaySound.audios.PlayMusic ("Tension Ball", true, 0.25f);
    }


    private void TerminarRecorrido()
    {
	    blockerButtons.SetActive(false);

	    string numberSelected = ball.ballName;
	    string colorString = "Verde";
		
	    NumberColor color = NumberColor.Green;
	    if (numberSelected.Contains("Negro"))
	    {
		    color = NumberColor.Black;
		    colorString = "Negro";
	    }
	    else if (numberSelected.Contains("Rojo"))
	    {
		    color = NumberColor.Red;
		    colorString = "Rojo";
	    }

	    numberSelected = numberSelected.Replace("Negro", string.Empty);
	    numberSelected = numberSelected.Replace("Rojo", string.Empty);

		
	    int numberAsInt = int.Parse(numberSelected);
	    if (numberAsInt == 1 || numberAsInt == 24)
	    {
		    color = NumberColor.Green;
		    colorString = "Verde";
	    }

	    PlaySound.audios.PlayMusic("Lobby", true, 0.15f);
	    display.material.mainTexture = Resources.Load("LCD_Display/" + numberSelected+""+colorString) as Texture;

	    StartCoroutine (ShowFinalUI(numberAsInt,color));
    }
    private IEnumerator ShowFinalUI(int numberAsInt, NumberColor color)
    {
	    skipButton.gameObject.SetActive(false);
	    yield return new WaitForSeconds(0.5f);
	    animatorCamera.Play ("MostrarLCD");
	    yield return new WaitForSeconds(0.5f);
	    PlaySound.audios.PlayFX(numberAsInt.ToString());
	    PlaySound.audios.PlayFX(color.ToString(), 1, PlaySound.audios.GetFxClipLength(numberAsInt.ToString()));
	    GlobalObjects.BetResult?.Invoke(numberAsInt,color.ToString());
		    
	    yield return new WaitForSeconds(2f);

	    winScreen.ShowScreen();
	    yield return new WaitForSeconds(5f);
	    EndGame();
    }
    
    private void EndGame ()
    {
	    GenericTransaction<EndGameRequest> request = new GenericTransaction<EndGameRequest> ();
	    request.msgName = "endGame";
	    request.msgDescrip = new EndGameRequest ();

	    string json = JsonUtility.ToJson (request);
	    WebServiceManager.Instance.SendJsonData<GenericTransaction<EndGameResponse>> (XMLManager.UrlGeneric, json, "authorization", GlobalObjects.BackendToken, EndGameResponse, TransactionFailed);
    }

    private void EndGameResponse (GenericTransaction<EndGameResponse> response)
    {
	    GlobalObjects.UserMoneyReal = response.msgDescrip.balance + response.msgDescrip.bonusNonrestricted + response.msgDescrip.bonusRestricted;
	    canvasUI.SetActive(true);
	    main.enabled = false;
	    winScreen.gameObject.SetActive(false);
    }

    private void TransactionFailed (string errorMessage)
    {
	    Alert.Instance.Show ("Login Failed", errorMessage);
    }
}