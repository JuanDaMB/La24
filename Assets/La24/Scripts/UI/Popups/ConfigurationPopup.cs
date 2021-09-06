using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ConfigurationPopup : MonoBehaviour
{
	[SerializeField]
	private AudioMixer mixer;

	[SerializeField]
	private Toggle tglSound;

	[SerializeField]
	private Toggle tglFx;

	[SerializeField]
	private Toggle tglAlert;

	private void Start()
	{
		tglSound.onValueChanged.AddListener(OnSoundValueChanged);
		tglFx.onValueChanged.AddListener(OnFXValueChanged);
		tglAlert.onValueChanged.AddListener(OnAlertValueChanged);
	}

	private void OnSoundValueChanged(bool value)
	{
		mixer.SetFloat("MusicVolume", value ? 0 : -80);
		PlaySoundValueChanged(value);
	}

	private void OnFXValueChanged(bool value)
	{
		mixer.SetFloat("FXVolume", value ? 0 : -80);
		PlaySoundValueChanged(value);
	}

	private void OnAlertValueChanged(bool value)
	{
		PlaySoundValueChanged(value);
	}

	private void PlaySoundValueChanged(bool value)
	{
		PlaySound.audios.PlayFX(value ? "Toggle On" : "Toggle Off");
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}