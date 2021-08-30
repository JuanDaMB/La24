using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace IndieLevelStudio.Common
{
	public class HintManager : MonoBehaviour
	{
		[SerializeField]
		private Text message;

		[SerializeField]
		private Text title;

		[SerializeField]
		private LeanTweenType easeType;

		private float hintReadDelay;

		private RectTransform rectTransform;

		private Vector3 initPos;

		private Vector3 hidePos;

		private List<string> texts;

		private int index;

		private bool isPlaying;

		public void Configure (float hintsTime)
		{
			rectTransform = GetComponent<RectTransform> ();
			initPos = rectTransform.anchoredPosition;
			hidePos = initPos + (Vector3.up * 475);

			rectTransform.anchoredPosition = hidePos;
			title.text = XMLManager.UserMessagesTitle;

			hintReadDelay = hintsTime;
			texts = new List<string> ();
		}

		public void ShowHints (List<string> texts)
		{
			if (texts.Count == 0)
				return;

			index = 0;
			this.texts.AddRange (texts);

			if (!isPlaying) {
				rectTransform.anchoredPosition = hidePos;
				ShowTextQueue ();
			}
		}

		private void ShowTextQueue ()
		{
			PlaySound.audios.PlayFX ("Entrada de Mensajes", 0.25f);
			isPlaying = true;

			message.text = texts [index];
			LeanTween.move (rectTransform, initPos, 0.5f).setEase (easeType).setOnComplete (() => {
				LeanTween.move (rectTransform, hidePos, 0.5f).setOnComplete (ShowNextHint).setDelay (hintReadDelay);
			});
		}

		private void ShowNextHint ()
		{
			index++;
			if (index >= texts.Count) {
				isPlaying = false;
				texts.Clear ();
				return;
			}
			ShowTextQueue ();
		}
	}
}