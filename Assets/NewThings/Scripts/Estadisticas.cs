using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IndieLevelStudio.Common;
using IndieLevelStudio.Networking;
using IndieLevelStudio.Networking.Models;
using TMPro;
using UnityEngine;

public class Estadisticas : MonoBehaviour
{
    [SerializeField] private EstadisticasBar[] barras;
    [SerializeField] private TextMeshProUGUI[] ultimos, calientes, frios;
    [SerializeField] private Color[] _colors;

    private List<int> _last, _cold, _hot;


    public void SetUp()
    {
        _last = new List<int>();
        _cold = new List<int>();
        _hot = new List<int>();
        foreach (EstadisticasBar bar in barras)
        {
            bar.SetUp();
        }
    }

    public void Hide()
    {
        GlobalObjects.State = GlobalObjects.PreviousState;
        gameObject.SetActive(false);
    }

    public void StadisticasRequest()
    {
        GenericTransaction<StadisticsRequest> request = new GenericTransaction<StadisticsRequest>();
        request.msgName = "getStat";
        request.msgDescrip = new StadisticsRequest();

        string json = JsonUtility.ToJson(request);

        WebServiceManager.Instance.SendJsonData<GenericTransaction<StadisticsResponse>>(XMLManager.UrlGeneric, json,
            "authorization", GlobalObjects.BackendToken, GetStatsResponse, null);
    }

    private void GetStatsResponse(GenericTransaction<StadisticsResponse> response)
    {
        GlobalObjects.SaveTransactionAttributes(response);
        Show(response.msgDescrip.last, response.msgDescrip.hot, response.msgDescrip.cold);
    }

    public void Show(List<int> last, List<int> hot, List<int> cold)
    {
        _last.Clear();
        _hot.Clear();
        _cold.Clear();

        _last = last;
        _hot = hot;
        _cold = cold;

        GlobalObjects.PreviousState = GlobalObjects.State;
        GlobalObjects.State = GameState.Pause;
        gameObject.SetActive(true);
        Dibujar();
    }

    private void Dibujar()
    {
        foreach (EstadisticasBar bar in barras)
        {
            bar.ResetValues();
        }

        int index = 0;

        for (int i = 0; i < _last.Count; i++)
        {
            index = NumberColor(_last[_last.Count - i - 1]);
            if (_last[_last.Count - i - 1] > 24)
            {
                barras[_last[_last.Count - i - 1] - 25].AddValue(index % 2);
            }
            else
            {
                barras[_last[_last.Count - i - 1] - 1].AddValue(index % 2);
            }
        }


        for (int i = 0; i < ultimos.Length && i < _last.Count; i++)
        {
            ultimos[i].color = _colors[NumberColor(_last[_last.Count - i - 1])];
            if (_last[_last.Count - i - 1] > 24)
            {
                _last[_last.Count - i - 1] -= 24;
            }

            ultimos[i].text = _last[_last.Count - i - 1].ToString();
        }

        foreach (EstadisticasBar bar in barras)
        {
            bar.Draw();
        }

        for (int i = 0; i < _hot.Count; i++)
        {
            calientes[i].color = _colors[NumberColor(_hot[i])];

            if (_hot[i] > 24)
            {
                _hot[i] -= 24;
            }

            calientes[i].text = _cold[i].ToString();
        }

        for (int i = 0; i < _cold.Count; i++)
        {
            frios[i].color = _colors[NumberColor(_cold[i])];
            if (_cold[i] > 24)
            {
                _cold[i] -= 24;
            }

            frios[i].text = _cold[i].ToString();
        }
    }

    private int NumberColor(int number)
    {
        if (number % 24 == 0 || number % 24 == 1)
        {
            return 2;
        }

        return number > 24 ? 0 : 1;
    }
}
