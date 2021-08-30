using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlaySound : MonoBehaviour
{
	public static PlaySound audios;

	[SerializeField]
	private List<AudioClip> fxClips;

	[SerializeField]
	private List<AudioClip> ambiencesClips;

	[SerializeField]
	private List<AudioClip> musicClips;

	[SerializeField]
	private AudioSource source;

	[SerializeField]
	private AudioSource ambienceSource;

	[SerializeField]
	private AudioSource mainMusic;

	private float FadeTime = 0.5f;

	private void Awake ()
	{
		audios = this;
	}

	public void PlayFX (string audioName, float volume = 1, float delay = 0)
	{
		if (!string.IsNullOrEmpty (audioName) && audios != null)
			OnPlayFX (audioName, volume, delay);
	}

	public void PlayAmbientance (string audioName, float volume = 1, float delay = 0, bool isLoop = false)
	{
		if (!string.IsNullOrEmpty (audioName) && audios != null)
			OnPlayAmbientance (audioName, volume, delay, isLoop);
	}

	public void PlayMusic (string audioName, bool loop = true, float volume = 1)
	{
		if (!string.IsNullOrEmpty (audioName) && audios != null)
			OnPlayMusic (audioName, loop, volume);
	}

	public float GetFxClipLength (string audioName)
	{
		if (audios == null)
			return -1;

		float length = 0;
		AudioClip clip = audios.fxClips.Find (a => a.name == audioName);
		if (clip != null)
			length = clip.length;
		else
			Debug.LogWarning ("Audio FX " + audioName + " is not defined in manager");

		return length;
	}

	public void StopFX ()
	{
		if (audios != null)
			source.Stop ();
	}

	public void StopMusic ()
	{
		if (audios != null)
			mainMusic.Stop ();
	}

	public void StopAmbiences ()
	{
		if (audios != null)
			ambienceSource.Stop ();
	}

	private void OnPlayFX(string audioName, float volume, float delay)
	{
		AudioClip clip = fxClips.Find(c => c.name == audioName);
		if (!fxClips.Exists(c => c.name == audioName)) return;
		if (delay != 0)
		{
			StartCoroutine(CoPlayDelayedClip(clip,volume, delay));
		}
		else
		{
			source.PlayOneShot(clip, volume);
		}
	}
	private IEnumerator CoPlayDelayedClip( AudioClip _clip, float volume, float _delay) {
		yield return new WaitForSeconds(_delay);
		source.PlayOneShot(_clip,volume);
	}

	private void OnPlayAmbientance (string audioName, float volume, float delay, bool loop)
	{
		AudioClip clip = ambiencesClips.Find (c => c.name == audioName);
		if (clip != null) {
			ambienceSource.Stop ();
			ambienceSource.clip = clip;
			ambienceSource.volume = volume;
			ambienceSource.loop = loop;
			ambienceSource.PlayDelayed (delay);
		} else
			Debug.LogWarning ("Audio ambientance " + audioName + " is not defined in manager");
	}

	private void OnPlayMusic (string audioName, bool loop, float volume)
	{
		AudioClip clip = musicClips.Find (c => c.name == audioName);
		if (clip != null) {
			LeanTween.value (gameObject, (val) => {
				mainMusic.volume = val;
			}, volume, 0, FadeTime).setOnComplete (() => {
				mainMusic.Stop ();
				mainMusic.clip = clip;
				mainMusic.volume = volume;
				mainMusic.loop = loop;
				mainMusic.Play ();

				LeanTween.value (gameObject, (val) => {
					mainMusic.volume = val;
				}, 0, volume, FadeTime);
			});
		} else
			Debug.LogWarning ("Music " + audioName + " is not defined in manager");
	}
}