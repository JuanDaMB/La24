using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColorManager : MonoBehaviour
{
    [SerializeField] private List<Image> objects;
    [SerializeField] private List<Sprite> azul, verde;
    [SerializeField] private UserData Data;


    public void ChangeColor(SceneColor currentColor)
    { 
        Data.ChangeColor(currentColor);
        for (int i = 0; i < objects.Count; i++)
        {
            switch (currentColor)
            {
                case SceneColor.Azul:
                    objects[i].sprite = azul[i];
                    break;
                case SceneColor.Verde:
                    objects[i].sprite = verde[i];
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentColor), currentColor, null);
            }   
        }
    }
    
}

public enum SceneColor
{
    Verde = 0,
    Azul = 1
}