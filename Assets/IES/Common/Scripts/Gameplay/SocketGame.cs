using System;
using IndieLevelStudio.Common;
using UnityEngine;

namespace IndieLevelStudio.Networking
{
	public class SocketGame : SocketBase
	{
		public event Action<string> SocketConnectionFail;

		[SerializeField]
		public float sendMessageDelay;

		[SerializeField]
		public float waitToResponse;

		private int reconnectionTries;

		private void Awake()
		{
			OnSocketTextReceived += MessageReceived;
			OnSocketConnection += SocketConnection;
		}

		private void SocketConnection(bool connected)
		{
			LeanTween.cancel(gameObject);
			if (connected)
				LeanTween.delayedCall(gameObject, sendMessageDelay, SendingMessage);
			else
				TryConnect();
		}

		private void TryConnect()
		{
			LeanTween.cancel(gameObject);
			Disconnect();

			reconnectionTries++;
			LogMessage("Trying to reconnect socket, try count:" + reconnectionTries, true);

			if (reconnectionTries <= 5)
				InitializeSocket(XMLManager.SocketURL);
			else
				TryNotifyUser();
		}

		private void TryNotifyUser()
		{
			if (SocketConnectionFail != null)
				SocketConnectionFail("Sobrepasó número de reintentos de conexión");
		}

		private void SendingMessage()
		{
			SendString("Test");
			reconnectionTries = 0;

			LeanTween.delayedCall(gameObject, waitToResponse, SocketPresumablyDisconnected);
		}

		private void SocketPresumablyDisconnected()
		{
			LeanTween.cancel(gameObject);
			LogMessage("Not message arrived for a long time", true);

			TryConnect();
		}

		private void MessageReceived(string message)
		{
			LeanTween.cancel(gameObject);
			LeanTween.delayedCall(gameObject, sendMessageDelay, SendingMessage);
		}
	}
}