using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Estadisticas : MonoBehaviour
{
    [SerializeField] private EstadisticasBar[] barras;
    [SerializeField] private TextMeshProUGUI[] ultimos, calientes, frios;
    [SerializeField] private Color[] _colors;
    private Queue<(NumberColor,int)> betDatas;
    private Dictionary<(NumberColor, int), int> betValues;

    public string PreviousBetString
    {
        get => PlayerPrefs.GetString("PREVIOUSS_BETS_JSON", string.Empty);
        set => PlayerPrefs.SetString("PREVIOUSS_BETS_JSON", value);
    }

    public void SetUp()
    {
        betDatas = new Queue<(NumberColor, int)>();
        betValues = new Dictionary<(NumberColor, int), int>();
        foreach (EstadisticasBar bar in barras)
        {
            bar.SetUp();
        }
        Dibujar();
        // CargarDatos();
    }

    public void Show()
    {
        Dibujar();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        // GuardarDatos();
        gameObject.SetActive(false);
    }
    private void GuardarDatos()
    {
        List<(NumberColor,int)> list = new List<(NumberColor, int)>(betDatas);
        PreviousBetString = JsonUtility.ToJson(list);
    }

    private void CargarDatos()
    {
        if (string.IsNullOrEmpty(PreviousBetString)) return;
        
        List<(NumberColor,int)> list = new List<(NumberColor, int)>(JsonUtility.FromJson<List<(NumberColor,int)>>(PreviousBetString));
        betDatas = new Queue<(NumberColor, int)>(list);
        Dibujar();
    }

    private void Dibujar()
    {
        foreach (EstadisticasBar bar in barras)
        {
            bar.ResetValues();
        }

        int index = 0;

        foreach ((NumberColor, int) data in betDatas)
        {
            ultimos[index].text = data.Item2.ToString();
            ultimos[index].color = _colors[(int)data.Item1];
            if (data.Item1 == NumberColor.Green)
            {
                barras[data.Item2-1].AddValue(0);   
            }
            else
            {
                barras[data.Item2-1].AddValue((int)data.Item1);   
            }
            if (betValues.ContainsKey(data))
            {
                betValues[data]++;
            }
            else
            {
                betValues.Add(data,1);
            }

            index++;
        }

        foreach (EstadisticasBar bar in barras)
        {
            bar.Draw();
        }

        index = 0;
        foreach (KeyValuePair<(NumberColor, int),int> pair in betValues.OrderByDescending(t => t.Value).Take(calientes.Length))
        {
            calientes[index].text = pair.Key.Item2.ToString();
            calientes[index].color = _colors[(int)pair.Key.Item1];
            index++;
        }

        index = 0;
        foreach (KeyValuePair<(NumberColor, int),int> pair in betValues.OrderBy(t => t.Value).Take(frios.Length))
        {
            frios[index].text = pair.Key.Item2.ToString();
            frios[index].color = _colors[(int)pair.Key.Item1];
            index++;
        }
    }

    public void SumarDatos(int numberAsInt, NumberColor color)
    {
        betValues.Clear();
        betDatas.Enqueue((color,numberAsInt));
        if (betDatas.Count >ultimos.Length)
        {
            betDatas.Dequeue();
        }
        // GuardarDatos();
        Dibujar();
    }
}
