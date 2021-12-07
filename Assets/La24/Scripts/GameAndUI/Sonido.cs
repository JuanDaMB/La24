using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Sonido : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Image _slider;

    private float volume;
    public void StepVolume(float step)
    {
        volume += step;
        SetVolume(volume);
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
        if (this.volume <= 0f)
        {
            this.volume = 0.0001f;
        }

        if (this.volume >= 1f)
        {
            this.volume = 1f;
        }
        _slider.fillAmount = this.volume;

        _mixer.SetFloat("MusicVol", Mathf.Log10(this.volume)*20);
    }
}
