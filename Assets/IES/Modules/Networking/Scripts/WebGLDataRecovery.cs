using UnityEngine;
using UnityEngine.SceneManagement;
using IndieLevelStudio.Common;
using System;
using System.Collections.Generic;

namespace IndieLevelStudio.Networking
{
	public class WebGLDataRecovery : MonoBehaviour
	{
		[SerializeField]
		private string demoURL;

		[SerializeField]
		private List<WebSpecialChar> specialChars;

		private string url;

		private string port;

		private string protocol;

		private void Start ()
		{
			#if UNITY_WEBGL
			SetGameAttributes (Application.isEditor ? demoURL : Application.absoluteURL);
			#endif
			LoadGame ();
		}

		private void SetGameAttributes (string absoluteUrl)
		{
			int index = absoluteUrl.LastIndexOf ("?");

			string splittedUrl = absoluteUrl.Substring (index + 1);
			string[] parameters = splittedUrl.Split ('&');

			string data = string.Empty;
			string value = string.Empty;

			foreach (string param in parameters) {
				data = param.Split ('=') [0];
				value = param.Split ('=') [1];

				switch (data.ToLower ()) {
				case "token":
					GlobalObjects.token = ReplaceSpecialCharacters (value);
					break;

				case "gpuser":
					GlobalObjects.gpUser = ReplaceSpecialCharacters (value);
					break;

				case "sysoperator":
					GlobalObjects.sysOperator = ReplaceSpecialCharacters (value);
					break;

				case "player":
					GlobalObjects.player = ReplaceSpecialCharacters (value);
					break;

				case "game":
					GlobalObjects.game = ReplaceSpecialCharacters (value);
					break;

				case "minigame":
					GlobalObjects.miniGame = ReplaceSpecialCharacters (value);
					break;

				case "ip":
					url = ReplaceSpecialCharacters (value);
					break;

				case "port":
					port = ReplaceSpecialCharacters (value);
					break;

				case "protocol":
					protocol = ReplaceSpecialCharacters (value);
					break;
				}
				Debug.Log (param);
			}
			string formattedUrl = protocol + "://" + url + ":" + port;

			XMLManager.UrlLogin = formattedUrl + "/ggbies/api/authenticate";
			XMLManager.UrlGeneric = formattedUrl + "/ggbies/api/v1/ggb";

			GlobalObjects.IsWebGL = true;
		}

		private string ReplaceSpecialCharacters (string text)
		{
			string formattedText = text;
			foreach (WebSpecialChar wsp in specialChars)
				formattedText = text.Replace (wsp.specialChar, wsp.replacement);
			
			return formattedText;
		}

		private void LoadGame ()
		{
			SceneManager.LoadScene (1);
		}
	}

	[Serializable]
	public class WebSpecialChar
	{
		public string specialChar;
		public string replacement;
	}
}