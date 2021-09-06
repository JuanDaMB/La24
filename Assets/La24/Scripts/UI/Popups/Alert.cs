using UnityEngine;
using UnityEngine.UI;
using System;
using IndieLevelStudio.Networking.Models;

public class Alert : MonoBehaviour
{
	public static Alert Instance;

	[SerializeField]
	private Text title;

	[SerializeField]
	private Text content;

	public void Setup ()
	{
		Instance = this;
	}

	public void Show (string title, string content, float hideTime = 0)
	{
		this.title.text = title;
		this.content.text = FormatedContent (content);

		gameObject.SetActive (true);

		if (IsInvoking ("Hide"))
			CancelInvoke ("Hide");

		if (hideTime > 0)
			Invoke ("Hide", hideTime);
	}

	private string FormatedContent (string message)
	{
		string formated = string.Empty;
		try {
			ErrorTransaction t = JsonUtility.FromJson<ErrorTransaction> (message);
			formated = t.title;
		} catch (Exception) {
			formated = message;
		}
		return formated;
	}

	private void Hide ()
	{
		gameObject.SetActive (false);
	}

	private void OnDestroy ()
	{
		Instance = null;
	}
}