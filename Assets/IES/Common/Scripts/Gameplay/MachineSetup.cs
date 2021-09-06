using System;
using IndieLevelStudio.Common;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace IndieLevelStudio.Ruleta.Gameplay
{
	public class MachineSetup : MonoBehaviour
	{
		[SerializeField]
		private AudioMixer mixer;

		[SerializeField]
		private Button btnInstructions;

		[SerializeField]
		private Button btnHideInstructions;

		[SerializeField]
		private Button btnShow;

		[SerializeField]
		private Button btnHide;

		[SerializeField]
		private Button btnGetBalance;

		[SerializeField]
		private Toggle toggleSounds;

		[SerializeField]
		private GameObject instructions;

		[SerializeField]
		private CountdownTimer betsTimer;

		[Header("Machine phisic buttons")]
		[SerializeField]
		private Button btnCashOut;

		[SerializeField]
		private Button btnPlay;

		[SerializeField]
		private Button btnExit;

		private RectTransform rt;

		private Vector3 initPos;

		private Vector3 hidePos;

		private bool isVisible;

		private bool IsSoundEnabled
		{
			set
			{
				PlayerPrefs.SetInt("SOUND", Convert.ToInt32(value));
			}
			get { return Convert.ToBoolean(PlayerPrefs.GetInt("SOUND", 1)); }
		}

		public void Setup()
		{
			rt = GetComponent<RectTransform>();

			btnExit.onClick.AddListener(QuitGame);
			btnGetBalance.onClick.AddListener(UpdateBalance);
			btnInstructions.onClick.AddListener(ShowInstructions);
			toggleSounds.onValueChanged.AddListener(SoundInteraction);
			btnHideInstructions.onClick.AddListener(HideInstructions);

			btnShow.onClick.AddListener(Show);
			btnHide.onClick.AddListener(Hide);

			SoundInteraction(IsSoundEnabled);
			toggleSounds.onValueChanged.AddListener(SoundInteractive);

			initPos = rt.anchoredPosition;
			hidePos = initPos + (Vector3.up * 400);
			isVisible = true;

			rt.anchoredPosition = hidePos;

#if UNITY_WEBGL
			btnExit.gameObject.SetActive(false);
			btnCashOut.gameObject.SetActive(false);
#endif

			GameplayManager.Instance.OnButtonPressed += SocketButtonIntegration;
		}

		private void SocketButtonIntegration(int btnIndex)
		{
			switch (btnIndex)
			{
				case 1:
					TryCashOut();
					break;

				case 9:
					TryPlay();
					break;

				case 10:
					TryExit();
					break;
			}
		}

		private void TryPlay()
		{
			if (btnPlay.gameObject.activeSelf && btnPlay.gameObject.activeInHierarchy && btnPlay.interactable)
				btnPlay.onClick.Invoke();
		}

		private void TryCashOut()
		{
			if (btnCashOut.gameObject.activeSelf && btnCashOut.gameObject.activeInHierarchy && btnCashOut.interactable)
				btnCashOut.onClick.Invoke();
		}

		private void TryExit()
		{
			if (btnExit.gameObject.activeSelf && btnExit.gameObject.activeInHierarchy && btnExit.interactable && isVisible)
				btnExit.onClick.Invoke();
		}

		private void SoundInteractive(bool isOn)
		{
			PlaySound.audios.PlayFX("Sonido Fichas", 0.5f);
		}

		private void QuitGame()
		{
			GameplayManager.Instance.socket.SendString("MENU");
			Debug.Log("Closing game " + GlobalObjects.game);
			Application.Quit();
		}

		private void ShowInstructions()
		{
			instructions.SetActive(true);
			if (betsTimer.HasStarted)
				betsTimer.Pause();
		}

		private void HideInstructions()
		{
			instructions.SetActive(false);
			if (betsTimer.HasStarted)
				betsTimer.Resume();
		}

		private void UpdateBalance()
		{
			btnGetBalance.interactable = false;
			GameplayManager.Instance.GetBalance(() => btnGetBalance.interactable = true);
		}

		private void SoundInteraction(bool enabled)
		{
			IsSoundEnabled = enabled;

			toggleSounds.isOn = IsSoundEnabled;
			toggleSounds.targetGraphic.enabled = !IsSoundEnabled;

			mixer.SetFloat("MusicVolume", IsSoundEnabled ? 0 : -80);
			mixer.SetFloat("FXVolume", IsSoundEnabled ? 0 : -80);

			Debug.Log("Sound enabled: " + IsSoundEnabled);
		}

		public void Show()
		{
			isVisible = false;
			LeanTween.move(rt, initPos, 0.25f);
		}

		public void Hide()
		{
			isVisible = true;
			LeanTween.move(rt, hidePos, 0.25f);
		}
	}
}