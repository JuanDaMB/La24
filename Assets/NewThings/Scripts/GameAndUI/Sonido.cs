using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Sonido : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Image _slider;

    public void SetVolume(float value)
    {
        _mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        _slider.fillAmount = value;
    }
}
