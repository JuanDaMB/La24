
using System.Collections;
using System.Collections.Generic;
using IndieLevelStudio.Common;
using IndieLevelStudio.Networking;
using IndieLevelStudio.Networking.Models;
using UnityEngine;

public class CashoutHandler : MonoBehaviour
{
	[SerializeField] public SocketGame socket;
	[SerializeField] private GameObject CashoutContainer;
    public void CashOut()
	{
		if (GlobalObjects.UserBet <= 0)
		{
			if (GlobalObjects.UserMoneyReal <= 0)
			{
				EnterCashoutMode (false);
				if (socket.SocketRunning) {
					socket.SendString("UNLOCK");
				}
				return;
			}

			CashoutContainer.SetActive(true);
			GameManager.Manager.GetBalance();
			EnterCashoutMode (true);

			if (socket.SocketRunning)
			{
				socket.SendString("BTN1");
			}
		}
	}
	
	public void ContinueCashOut()
	{
		CashoutContainer.SetActive(false);
		ConfirmCashout();
	}

	public void EndCashout()
	{
		CashoutContainer.SetActive(false);
			
		EnterCashoutMode (false);
		
		if (socket.SocketRunning) {
			socket.SendString("UNLOCK");
		}
	}

	public void ConfirmCashout()
	{
		GenericTransaction<CashOutRequest> request = new GenericTransaction<CashOutRequest> ();
		request.msgName = "cashOut";
		request.msgDescrip = new CashOutRequest ();

		request.msgDescrip.deno = GlobalObjects.Deno;

		EnterCashoutMode (true);
		if (socket.SocketRunning) {
			Debug.Log ("Sending cashout");
			socket.SendString ("CASHOUT");
		}

		string json = JsonUtility.ToJson (request);
		WebServiceManager.Instance.SendJsonData<GenericTransaction<CashOutResponse>> (XMLManager.UrlGeneric, json, "authorization", GlobalObjects.BackendToken, CashOutResponse, TransactionFailed);
	}

	public void CashOutResponse (GenericTransaction<CashOutResponse> response)
	{
		GlobalObjects.SaveTransactionAttributes (response);
		EnterCashoutMode (true);
	}

	public void EnterCashoutMode (bool cashout)
	{
		GlobalObjects.IsCashOutMode = cashout;
		//TODO blockerButtons.SetActive (GlobalObjects.IsCashOutMode);

		Debug.Log ("Cash out mode: " + GlobalObjects.IsCashOutMode);
	}

	private void TransactionFailed (string errorMessage)
	{
		Alert.Instance.Show ("Login Failed", errorMessage);
	}
}
