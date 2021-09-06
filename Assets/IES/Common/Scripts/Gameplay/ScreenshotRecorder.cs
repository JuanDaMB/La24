using System.Collections;
using UnityEngine;
using System.IO;
using System;

namespace IndieLevelStudio.Common
{
	public class ScreenshotRecorder
	{
		public static IEnumerator SaveScreenshot (string directory, float initialDelay, Action onCompleteScreenshot = null, bool isDoubleUp = false)
		{
			if (string.IsNullOrEmpty (directory) || GlobalObjects.IsWebGL) {
				if (onCompleteScreenshot != null)
					onCompleteScreenshot ();
				yield break;
			}
			yield return new WaitForSeconds (initialDelay);

			yield return new WaitForEndOfFrame ();

			try {
				byte[] bytes = ScreenCapture.CaptureScreenshotAsTexture ().EncodeToJPG (15);

				if (!Directory.Exists (directory))
					Directory.CreateDirectory (directory);

				int scIndex = 0;
				int playSession = isDoubleUp ? GlobalObjects.idPlaysessionDouble : GlobalObjects.idPlaysession;
				string fileName = playSession.ToString ("0000") + "_" + GlobalObjects.game + scIndex.ToString ("00") + ".jpg";
				string filePath = directory + "/" + fileName;


				while (File.Exists (filePath)) {
					scIndex++;
					fileName = playSession.ToString ("0000") + "_" + GlobalObjects.game + "" + scIndex.ToString ("00") + ".jpg";
					filePath = directory + "/" + fileName;
				}

				File.WriteAllBytes (filePath, bytes);
				Debug.Log ("Saved Screenshot: " + filePath);
			} catch (Exception e) {
				Debug.LogWarning ("Cannot save screenshot, reason: " + e.Message);
			}
			if (onCompleteScreenshot != null)
				onCompleteScreenshot ();
		}

		public static Texture2D LoadScreenshot (string screenshotName)
		{
			string path = Application.persistentDataPath + "/../ScreenShots/" + screenshotName + ".jpg";
			Texture2D tx = new Texture2D (1920, 1080);

			if (!File.Exists (path))
				return tx;

			byte[] buffer = File.ReadAllBytes (path);
			tx.LoadImage (buffer, false);

			return tx;
		}
	}
}

