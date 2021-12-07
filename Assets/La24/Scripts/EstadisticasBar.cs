using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class EstadisticasBar : MonoBehaviour
{
    [SerializeField] private Image[] barras;
    private int[] _cantidades;

    private void Start()
    {
        barras = new Image[GetComponentsInChildren<Image>().Length-1];
        for (int i = 1; i < GetComponentsInChildren<Image>().Length; i++)
        {
            barras[i-1] = GetComponentsInChildren<Image>()[i];
        }
    }

    public void Draw()
    {
        for (int i = 0; i < barras.Length; i++)
        {
            barras[i].fillAmount = _cantidades[i] * 0.2f;
        }
    }

    public void SetUp()
    {
        _cantidades = new int[barras.Length];
    }

    public void AddValue(int index)
    {
        _cantidades[index]++;
    }

    public void ResetValues()
    {
        for (int i = 0; i < _cantidades.Length; i++)
        {
            _cantidades[i] = 0;
        }
    }
}
