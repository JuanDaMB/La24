using UnityEngine;
using UnityEngine.UI;
using System;

namespace IndieLevelStudio.IES.SlotMinigames
{
	public class DuplicatePopup : MonoBehaviour
	{
		public event Action<bool> OnDuplicate;

		[SerializeField]
		private Text currentValue;

		[SerializeField]
		private Text valueAfterDuplicate;

		[SerializeField]
		public Button buttonYes;

		[SerializeField]
		public Button buttonNo;

		[SerializeField]
		private Image fillArrowImage;

		private float dupliatedValue;

		private string soundName;

		private bool moneyFormat;

		public void Setup ()
		{
			buttonYes.onClick.AddListener (YesDuplicate);
			buttonNo.onClick.AddListener (NotDuplicate);
		}

		private void OnEnable ()
		{
			GameplayManager.Instance.OnButtonPressed += ButtonPressed;
		}

		private void OnDisable ()
		{
			GameplayManager.Instance.OnButtonPressed -= ButtonPressed;
		}

		private void ButtonPressed (int btnNumber)
		{
			switch (btnNumber) {
			case 3:
				TryYes ();
				break;
			case 7:
				TryNo ();
				break;
			}
		}

		private void TryYes ()
		{
			if (buttonYes.gameObject.activeSelf && buttonYes.gameObject.activeInHierarchy && buttonYes.interactable)
				buttonYes.onClick.Invoke ();
		}

		private void TryNo ()
		{
			if (buttonNo.gameObject.activeSelf && buttonNo.gameObject.activeInHierarchy && buttonNo.interactable)
				buttonNo.onClick.Invoke ();
		}

		private void YesDuplicate ()
		{
			InteractButtons (false);
			StopAnimation ();

			if (OnDuplicate != null)
				OnDuplicate (true);
		}

		private void NotDuplicate ()
		{
			InteractButtons (false);
			StopAnimation ();

			if (OnDuplicate != null)
				OnDuplicate (false);
		}

		public void Show (float valueWinned, bool moneyFormat = false)
		{
			this.moneyFormat = moneyFormat;
			dupliatedValue = (valueWinned * 2);

			currentValue.text = "0";
			valueAfterDuplicate.text = "0";

			//GetClipName ();
			soundName = "Doblon Fastidio";
			gameObject.SetActive (true);

			float soundTime = PlaySound.audios.GetFxClipLength (soundName);

			LeanTween.value (valueAfterDuplicate.gameObject, FillDuplicate, 0, dupliatedValue, soundTime).setDelay (0.5f).setOnStart (PlayDoubetterSound);
			LeanTween.value (currentValue.gameObject, FillNormalValue, 0, valueWinned, soundTime).setDelay (0.5f);

			StartAnimation ();
		}

		private void FillDuplicate (float value)
		{
			valueAfterDuplicate.text = moneyFormat ? "$" + ((int)(value)).ToString ("N0") : ((int)(value)).ToString ("N0");
		}

		private void FillNormalValue (float value)
		{
			currentValue.text = moneyFormat ? "$" + ((int)(value)).ToString ("N0") : ((int)(value)).ToString ("N0");
		}

		private void PlayDoubetterSound ()
		{
			PlaySound.audios.PlayFX (soundName);
		}

		private void StopAnimation ()
		{
			LeanTween.cancel (valueAfterDuplicate.gameObject);
			LeanTween.cancel (currentValue.gameObject);

			PlaySound.audios.StopFX ();
		}

		private void StartAnimation ()
		{
			LeanTween.cancel (fillArrowImage.gameObject);
			fillArrowImage.fillAmount = 0;

			Animate ();
		}

		private void InteractButtons (bool interactable)
		{
			buttonYes.interactable = interactable;
			buttonNo.interactable = interactable;
		}

		private void Animate ()
		{
			LeanTween.value (fillArrowImage.gameObject, (val) => fillArrowImage.fillAmount = val, 0, 1, 1f).setDelay (0.5f).setOnComplete (Animate);
		}

		public void Hide ()
		{
			InteractButtons (true);
			gameObject.SetActive (false);
		}
	}
}