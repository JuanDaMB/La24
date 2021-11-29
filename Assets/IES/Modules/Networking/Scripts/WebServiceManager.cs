using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace IndieLevelStudio.Networking
{
	public class WebServiceManager : MonoBehaviour
	{
		public static WebServiceManager Instance {
			get;
			private set;
		}

		private void Awake ()
		{
			Instance = this;
		}

		public void SendJsonData<T> (string url, string json, string headerName, string headerValue, Action<T> onComplete, Action<string> onFailed)
		{
			StartCoroutine (Upload (url, json, headerName, headerValue, onComplete, onFailed));
		}

		private IEnumerator Upload<T> (string url, string data, string headerName, string headerValue, Action<T> onComplete, Action<string> onFailed)
		{
			Debug.Log ("Post: " + data + "\nUrl: " + url);
			byte[] bytes = Encoding.UTF8.GetBytes (data);

			UnityWebRequest www = UnityWebRequest.Post (url, string.Empty);
			www.uploadHandler = new UploadHandlerRaw (bytes);
			www.downloadHandler = new DownloadHandlerBuffer ();

			www.SetRequestHeader ("Content-Type", "application/json");
			if (!string.IsNullOrEmpty (headerName))
				www.SetRequestHeader (headerName, headerValue);

			// SetEnableLoading (true);
			yield return www.SendWebRequest ();

			if (!string.IsNullOrEmpty (www.error)) {
				Debug.LogWarning ("Failed response: " + www.error + " " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
				if (string.IsNullOrEmpty (www.downloadHandler.text))
					onFailed (www.error);
				else
					onFailed (www.downloadHandler.text);
			} else {
				if (www.responseCode == 200) {
					Debug.Log ("Succes response: " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
					onComplete (JsonUtility.FromJson<T> (www.downloadHandler.text));
				} else {
					Debug.LogWarning ("Failed response: " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
					onFailed (www.downloadHandler.text);
				}
			}
			// SetEnableLoading (false);
		}
	}
}