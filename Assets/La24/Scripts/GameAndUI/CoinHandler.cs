using System;
using System.Collections;
using System.Collections.Generic;
using NewThings.Scripts.GameAndUI;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class CoinHandler : MonoBehaviour
{
    [SerializeField] private List<Coin> Coins;
    [SerializeField] private List<CoinData> coinDatas;
    public Action<int> CoinChange;
    private int index = 0;

    private void CoinChanged(int value, int idx)
    {
        Coins[index].DeClicked();
        index = idx;
        Coins[index].Clicked();
        CoinChange?.Invoke(value);
    }

    public void ChangeCoinValues(int value)
    {
        foreach (Coin coin in Coins)
        {
            coin.ChangeText();
        }
    }

    public void AssingCoins(List<int> coins)
    {
        for (int i = 0; i < coinDatas.Count; i++)
        {
            coinDatas[i].Value = coins[i];
            Coins[i].SetData(coinDatas[i], i);
            Coins[i].Suscribe(CoinChanged);
        }
        CoinChanged(coinDatas[0].Value,0);
    }
}
