using System;
using IndieLevelStudio.BetsModule.Controllers;
using IndieLevelStudio.Common;
using IndieLevelStudio.Networking;
using IndieLevelStudio.Networking.Models;
using IndieLevelStudio.Ruleta.Gameplay;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
	public GameManager gameManager;

	private void Start()
	{
		XMLManager.LoadXML();
		CallLogin();
	}

	private void CallLogin()
	{
		LoginRequest request = new LoginRequest();
		request.username = XMLManager.UserName;
		request.password = XMLManager.UserPass;
		request.rememberMe = XMLManager.RememberUser;

		string json = JsonUtility.ToJson(request);
		WebServiceManager.Instance.SendJsonData<LoginResponse>(XMLManager.UrlLogin, json, string.Empty, string.Empty,
			LoginSucceded, TransactionFailed);
	}

	private void LoginSucceded(LoginResponse resp)
	{
		GlobalObjects.BackendToken = "Bearer " + resp.id_token;
		CallConfig();
	}

	private void CallConfig()
	{
		GenericTransaction<ConfigRequest> request = new GenericTransaction<ConfigRequest>();
		request.msgName = "getConfig";
		request.msgDescrip = new ConfigRequest();

		string json = JsonUtility.ToJson(request);

		WebServiceManager.Instance.SendJsonData<GenericTransaction<ConfigResponse>>(XMLManager.UrlGeneric, json,
			"authorization", GlobalObjects.BackendToken, GetCongfigResponse, TransactionFailed);
	}

	public void GetCongfigResponse(GenericTransaction<ConfigResponse> response)
	{
		GlobalObjects.SaveTransactionAttributes(response);

		GlobalObjects.IsRecoveryMode =
			response.msgDescrip.recovered.game != null && response.msgDescrip.recovered.game.Count > 0;

		if (GlobalObjects.IsRecoveryMode)
			GlobalObjects.RecoveryInfo = response.msgDescrip.recovered;

		gameManager.SetCoinAndDeno(response.msgDescrip.coins, response.msgDescrip.deno);
		GlobalObjects.MinBet = response.msgDescrip.minBet;
		GlobalObjects.MaxBet = response.msgDescrip.maxBet;

		gameManager.SetMaxBetValue();
		gameManager.ConfigureGame(response.msgDescrip.betTime);

		XMLManager.BetsTime = response.msgDescrip.betTime;

		GenericTransaction<PrizesTableRequest> request = new GenericTransaction<PrizesTableRequest>();
		request.msgName = "getPrizes";
		request.msgDescrip = new PrizesTableRequest();

		string json = JsonUtility.ToJson(request);

		WebServiceManager.Instance.SendJsonData<GenericTransaction<PrizesTableResponse>>(XMLManager.UrlGeneric, json,
			"authorization", GlobalObjects.BackendToken, GetPrizesResponse, TransactionFailed);
	}

	public void GetPrizesResponse(GenericTransaction<PrizesTableResponse> response)
	{
		GlobalObjects.SaveTransactionAttributes(response);

		GlobalObjects.PrizesData = response.msgDescrip;

		GenericTransaction<BalanceRequest> request = new GenericTransaction<BalanceRequest>();
		request.msgName = "getBalance";
		request.msgDescrip = new BalanceRequest();

		string json = JsonUtility.ToJson(request);

		WebServiceManager.Instance.SendJsonData<GenericTransaction<BalanceResponse>>(XMLManager.UrlGeneric, json,
			"authorization", GlobalObjects.BackendToken, GetBalanceSucceded, TransactionFailed);
	}

	private void GetBalanceSucceded(GenericTransaction<BalanceResponse> response)
	{
		GlobalObjects.SaveTransactionAttributes(response);

		GlobalObjects.UserMoneyReal = response.msgDescrip.balance + response.msgDescrip.bonusNonrestricted +
		                          response.msgDescrip.bonusRestricted;
		
		if (GlobalObjects.IsRecoveryMode)
			gameManager.RecoveryGame();
	}

	private void TransactionFailed(string errorMessage)
	{
		string message = FormatedContent(errorMessage);

		if (message == "La maquina se encuentra bloqueada")
		{
			gameManager.EnterCashoutMode(true);

			return;
		}

		Alert.Instance.Show("Fallo en autenticación", message);
	}

	private string FormatedContent(string message)
	{
		string formated = string.Empty;
		try
		{
			ErrorTransaction t = JsonUtility.FromJson<ErrorTransaction>(message);
			formated = t.title;
		}
		catch (Exception)
		{
			formated = message;
		}

		return formated;
	}
}