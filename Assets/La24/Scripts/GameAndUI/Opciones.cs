using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Opciones : MonoBehaviour
{
    [SerializeField] private Transform[] colores;
    [SerializeField] private Transform marco;
    [SerializeField] private ColorManager _colorManager;

    public void ColorTheme(int color)
    {
        marco.position = colores[color].position;
        _colorManager.ChangeColor((SceneColor) color);
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
