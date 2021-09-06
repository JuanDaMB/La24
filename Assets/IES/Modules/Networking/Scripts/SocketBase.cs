using System;
using System.Collections;
using IndieLevelStudio.IES.Poker.Models;
using UnityEngine;

namespace IndieLevelStudio.Networking
{
    public class SocketBase : MonoBehaviour
    {
        public event Action<SocketResponseMessage> OnSocketMessageReceived;

        public event Action<bool> OnSocketConnection;

        public event Action<string> OnSocketTextReceived;

        public event Action<string> OnSocketError;

        public string Url;

        [SerializeField]
        private bool logMessage;

        private WebSocket webSocket;

        private bool socketRunning;

        public bool SocketRunning
        {
            set
            {
                socketRunning = value;
                if (OnSocketConnection != null)
                    OnSocketConnection(socketRunning);
            }
            get { return socketRunning; }
        }

        public void InitializeSocket(string url)
        {
            Url = url;
            StartCoroutine(InitializeSockets());
        }

        private IEnumerator InitializeSockets()
        {
            webSocket = new WebSocket(new Uri(Url));

            yield return StartCoroutine(webSocket.Connect());

            SocketRunning = true;
            LogMessage("Socket initialized: " + name + "\nConnecting socket to " + Url);

            while (true)
            {
                if (!string.IsNullOrEmpty(webSocket.error))
                {
                    SocketRunning = false;
                    Debug.LogWarning("Error: " + webSocket.error);
                    if (OnSocketError != null)
                        OnSocketError(webSocket.error);

                    yield break;
                }

                string response = webSocket.RecvString();
                if (!string.IsNullOrEmpty(response))
                {
                    if (OnSocketTextReceived != null)
                        OnSocketTextReceived(response);
                }

                yield return null;
            }
        }

        public void SendString(string message)
        {
            webSocket.SendString(message);
        }

        public void Dispose()
        {
            OnSocketError = null;
            OnSocketMessageReceived = null;

            Disconnect();
            LogMessage(name + " socket closed");
        }

        public void Disconnect()
        {
            if (webSocket != null)
                webSocket.Close();
        }

        protected void LogMessage(object message, bool isWarning = false)
        {
            if (!logMessage)
                return;

            if (isWarning)
                Debug.LogWarning(message);
            else
                Debug.Log(message);
        }
    }
}