using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Opciones : MonoBehaviour
{
    [SerializeField] private Transform[] colores;
    [SerializeField] private Transform marco;
    [SerializeField] private ColorManager _colorManager;

    private int volume;


    public void ColorTheme(int color)
    {
        marco.position = colores[color].position;
        _colorManager.ChangeColor((SceneColor) color);
    }

    public void SetVolume(int volume)
    {
        this.volume -= volume;
        if (this.volume <= 0)
        {
            this.volume = 0;
        }

        if (this.volume >= 100)
        {
            this.volume = 100;
        }
    }
    
    public void Show()
    {
        GlobalObjects.PreviousState = GlobalObjects.State;
        GlobalObjects.State = GameState.Pause;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        GlobalObjects.State = GlobalObjects.PreviousState;
        gameObject.SetActive(false);
    }
}
