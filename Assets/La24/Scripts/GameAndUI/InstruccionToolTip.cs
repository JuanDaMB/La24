using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstruccionToolTip : MonoBehaviour
{
    [SerializeField] private List<InstruccionData> datas;

    [SerializeField] private RectTransform rect,bar;

    [SerializeField] private TextMeshProUGUI titulo, descripcion;
    private int _actualInfo = 0;

    public void SetData(int i)
    {
        _actualInfo = i;
        rect.gameObject.SetActive(true);
        rect.localPosition = datas[i].pos;
        bar.localPosition = datas[i].posBar;
        titulo.text = datas[i].titulo;
        descripcion.text = datas[i].descripcion;
    }

    public void HideData(int i)
    {
        if (i == _actualInfo)
        {
            rect.gameObject.SetActive(false);
        }
    }
    
}

[Serializable]
public struct InstruccionData
{
    public Vector2 pos,posBar;
    [TextArea]public string titulo,descripcion;
}