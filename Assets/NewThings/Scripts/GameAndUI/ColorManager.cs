using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    [SerializeField] private List<Image> objects;
    [SerializeField] private List<Sprite> azul, verde;

    public void ChangeColor(SceneColor currentColor)
    {
        
    }
    
}

public enum SceneColor
{
    Azul = 0,
    Verde = 1
}