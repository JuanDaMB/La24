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
		[SerializeField]
		private Text loaderText;

		[SerializeField]
		[Range (0, 2)]
		private float loaderAppearTime = 0;

		public static WebServiceManager Instance {
			get;
			private set;
		}

		private void Awake ()
		{
			Instance = this;
			SetEnableLoading (false);
		}

		public void GetDataFromEndpoint<K> (string url, string headerName, string headerValue, Action<K> onSucceded, Action<string> onFailed, bool isList = false) where K : class
		{
			StartCoroutine (CallGetData (url, headerName, headerValue, onSucceded, onFailed, isList));
		}

		public void GetDataFromEndpoint (string url, string headerName, string headerValue, Action<string> onSucceded, Action<string> onFailed)
		{
			StartCoroutine (CallGetData (url, headerName, headerValue, onSucceded, onFailed));
		}

		public void SendJsonData (string url, string json, string headerName, string headerValue, Action<string> onComplete, Action<string> onFailed)
		{
			StartCoroutine (Upload (url, json, headerName, headerValue, onComplete, onFailed));
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

			SetEnableLoading (true);
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
			SetEnableLoading (false);
		}

		private IEnumerator Upload (string url, string data, string headerName, string headerValue, Action<string> onComplete, Action<string> onFailed)
		{
			Debug.Log (data);
			byte[] bytes = Encoding.UTF8.GetBytes (data);

			UnityWebRequest www = UnityWebRequest.Post (url, string.Empty);
			www.uploadHandler = new UploadHandlerRaw (bytes);
			www.downloadHandler = new DownloadHandlerBuffer ();

			www.SetRequestHeader ("Content-Type", "application/json");
			if (!string.IsNullOrEmpty (headerName))
				www.SetRequestHeader (headerName, headerValue);

			SetEnableLoading (true);
			yield return www.SendWebRequest ();

			if (!string.IsNullOrEmpty (www.error))
				onFailed (www.error);
			else {
				if (www.responseCode == 200) {
					Debug.Log ("Succes response: " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
					onComplete (www.downloadHandler.text);
				} else {
					Debug.LogWarning ("Failed response: " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);
					onFailed (www.downloadHandler.text);
				}
			}
			SetEnableLoading (false);
		}

		private IEnumerator CallGetData<K> (string url, string headerName, string headerValue, Action<K> onSucceded, Action<string> onFailed, bool isList) where K : class
		{
			UnityWebRequest www = UnityWebRequest.Get (url);

			www.SetRequestHeader ("Content-Type", "application/json");

			if (!string.IsNullOrEmpty (headerName))
				www.SetRequestHeader (headerName, headerValue);

			SetEnableLoading (true);
			yield return www.SendWebRequest ();

			if (!string.IsNullOrEmpty (www.error)) {
				Debug.LogWarning ("Failed request: " + www.error + "\nurl: " + url);
				onFailed (www.error);
			} else {
				string json = isList ? "{\"data\":" + www.downloadHandler.text + "}" : www.downloadHandler.text;
				Debug.Log ("Succes response: " + json + "\nCode: " + www.responseCode + "\nUrl: " + url);

				if (www.responseCode == 200)
					onSucceded (JsonUtility.FromJson<K> (json));
				else
					onFailed (www.downloadHandler.text);
			}
			SetEnableLoading (false);
		}

		private IEnumerator CallGetData (string url, string headerName, string headerValue, Action<string> onSucceded, Action<string> onFailed)
		{
			UnityWebRequest www = UnityWebRequest.Get (url);
			www.SetRequestHeader (headerName, headerValue);

			SetEnableLoading (true);
			yield return www.SendWebRequest ();

			if (!string.IsNullOrEmpty (www.error)) {
				Debug.LogWarning ("Failed request: " + www.error + "\nurl: " + url);
				onFailed (www.error);
			} else {
				Debug.Log ("Succes response: " + www.downloadHandler.text + "\nCode: " + www.responseCode + "\nUrl: " + url);

				if (www.responseCode == 200)
					onSucceded (www.downloadHandler.text);
				else
					onFailed (www.downloadHandler.text);
			}
			SetEnableLoading (false);
		}

		private void SetEnableLoading (bool enabled)
		{
			// loaderText.text = string.Empty;
			// loaderText.enabled = enabled;
			//
			// if (enabled)
			// 	LeanTween.delayedCall (loaderText.gameObject, loaderAppearTime, WriteLoaderWord);
			// else
			// 	LeanTween.cancel (loaderText.gameObject);

		}

		private void WriteLoaderWord ()
		{
			loaderText.text = "";
		}
	}
}