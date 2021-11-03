using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NewThings.Scripts.GameAndUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetBoard : MonoBehaviour
{
    [SerializeField] private List<BetBoardButton> _buttons;
    [SerializeField] private List<Image> _images;
    [SerializeField] private List<CoinData> coinDatas;

    [SerializeField] private Color _blank, _color;

    private Dictionary<string, (int value, BetType type)> betValues;
    private Dictionary<string, (int value, BetType type)> lastBet;

    private List<(string key, int value, int index, BetType type)> betOrder;
    private List<(string key, int value, int index, BetType type)> lastBetOrder;

    public Action<bool> onBet;

    // Start is called before the first frame update
    void Start()
    {
        betValues = new Dictionary<string, (int, BetType)>();
        lastBet = new Dictionary<string, (int, BetType)>();
        betOrder = new List<(string key, int value, int index, BetType type)>();
        lastBetOrder = new List<(string key, int value, int index, BetType type)>();
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].Initialize(_blank,_color,_images[i], i, SetBetCoin);
        }
    }


    public bool CanBet()
    {
        int value = 0;
        if (betValues.Count <= 0)
        {
            return false;
        }
        foreach (KeyValuePair<string,(int value, BetType type)> pair in betValues)
        {
            value += pair.Value.value;
        }

        return value*GlobalObjects.Deno >= GlobalObjects.MinBet;
    }
    public Dictionary<string, (int value, BetType type)> GetBet()
    {
        if (betValues.Count <= 0) return betValues;
        lastBet = betValues.ToDictionary(t => t.Key, t => t.Value);
        lastBetOrder = betOrder.ToList();
        return betValues;
    }

    public void ReDoBet()
    {
        if (lastBetOrder.Count <= 0)
        {
            return;
        }

        foreach ((string key, int value, int index, BetType type) tuple in lastBetOrder)
        {
            SetBetCoin(tuple.index,tuple.key,tuple.value, tuple.type);
        }
        // GlobalObjects.UserBet = 0;
        // GlobalObjects.UserMoney = GlobalObjects.UserMoneyReal - GlobalObjects.UserBet;
        // foreach ((string key, int value, int index) i in lastBetOrder)
        // {
        //     foreach (var data in coinDatas.Where(data => data.Value == i.value))
        //     {
        //         _buttons[i.index].SetImage(data.sprite, true);
        //         break;
        //     }
        //     _buttons[i.index].SetText(lastBet[i.key].value.ToString());
        // }
        //
        //
        // betValues = lastBet.ToDictionary(t => t.Key, t => t.Value);
        // betOrder = lastBetOrder.ToList();
        //
        // foreach (KeyValuePair<string,(int value, BetType type)> i in betValues)
        // {
        //     GlobalObjects.UserBet += i.Value.value*GlobalObjects.Deno;
        // }
        // GlobalObjects.UserMoney = GlobalObjects.UserMoneyReal - GlobalObjects.UserBet;
        // onBet?.Invoke(true);
    }
    
    public void ClearBoard()
    {
        GlobalObjects.UserBet = 0;
        GlobalObjects.UserMoney = GlobalObjects.UserMoneyReal - GlobalObjects.UserBet;
        foreach (BetBoardButton button in _buttons)
        {
            button.ClearData();
        }
        betValues.Clear();
        betOrder.Clear();
        onBet?.Invoke(false);
    }

    public void ClearLast()
    {
        if (betOrder.Count <= 0) return;
        
        (string key, int value, int index, BetType type) value = betOrder[betOrder.Count-1];
        
        if (!betValues.ContainsKey(value.key)) return;
        
        betOrder.RemoveAt(betOrder.Count-1);
        betValues[value.key] = (betValues[value.key].value-value.value,betValues[value.key].type);
        
        GlobalObjects.UserBet -= value.value * GlobalObjects.Deno;
        GlobalObjects.UserMoney = GlobalObjects.UserMoneyReal - GlobalObjects.UserBet;
        _buttons[value.index].SetText((betValues[value.key].value).ToString());
        
        if (betValues[value.key].Item1 <= 0)
        {
            betValues.Remove(value.key);
            _buttons[value.index].SetImage(null, false);
        }
        else
        {
            int coin = 0;
            for (int i = betOrder.Count -1; i >= 0 ; i--)
            {
                if (betOrder[i].key != value.key) continue;
                coin = betOrder[i].value;
                break;
            }
            foreach (var data in coinDatas.Where(data => data.Value == coin))
            {
                _buttons[value.index].SetImage(data.sprite, true);
                break;
            }
        }

        if (betValues.Count <= 0)
        {
            onBet?.Invoke(false);
        }
    }

    private void SetBetCoin(int indexValue, string value, BetType type)
    {
        if (GlobalObjects.UserBet + (GlobalObjects.Coin*GlobalObjects.Deno) > GlobalObjects.UserMoneyReal)
        {
            return;
        }
        if (GlobalObjects.UserBet + (GlobalObjects.Coin*GlobalObjects.Deno) > GlobalObjects.MaxBet)
        {
            return;
        }
        
        onBet?.Invoke(true);
        if (!betValues.ContainsKey(value))
        {
            betValues.Add(value, (0,type));
        }
        var valueTuple = betValues[value];
        valueTuple.value += GlobalObjects.Coin;

        bool Assigned = false;

        foreach (CoinData data in coinDatas)
        {
            if (data.Value == GlobalObjects.Coin)
            {
                Assigned = true;
                _buttons[indexValue].SetImage(data.sprite, Assigned);
                break;
            }
        }
        
        if (!Assigned)
        {
            _buttons[indexValue].SetImage(null, Assigned);
        }
        _buttons[indexValue].SetText((valueTuple.value).ToString());
        
        betValues[value] = valueTuple;
        GlobalObjects.UserBet += GlobalObjects.Coin * GlobalObjects.Deno;
        GlobalObjects.UserMoney = GlobalObjects.UserMoneyReal - GlobalObjects.UserBet;
        betOrder.Add((value, GlobalObjects.Coin, indexValue, type));
    }
    private void SetBetCoin(int indexValue, string value, int coinValue, BetType type)
    {
        if (GlobalObjects.UserBet + (coinValue*GlobalObjects.Deno) > GlobalObjects.UserMoneyReal)
        {
            return;
        }
        if (GlobalObjects.UserBet + (GlobalObjects.Coin*GlobalObjects.Deno) > GlobalObjects.MaxBet)
        {
            return;
        }
        
        onBet?.Invoke(true);
        if (!betValues.ContainsKey(value))
        {
            betValues.Add(value, (0,type));  
        }
        var valueTuple = betValues[value];
        valueTuple.value += coinValue;

        bool Assigned = false;

        foreach (CoinData data in coinDatas)
        {
            if (data.Value == coinValue)
            {
                Assigned = true;
                _buttons[indexValue].SetImage(data.sprite, Assigned);
                break;
            }
        }
        
        if (!Assigned)
        {
            _buttons[indexValue].SetImage(null, Assigned);
        }
        _buttons[indexValue].SetText((valueTuple.value).ToString());
        
        betValues[value] = valueTuple;
        GlobalObjects.UserBet += coinValue * GlobalObjects.Deno;
        GlobalObjects.UserMoney = GlobalObjects.UserMoneyReal - GlobalObjects.UserBet;
        betOrder.Add((value, coinValue, indexValue, type));
    }
}

public enum BetType
{
    Par_Impar,
    Pleno,
    Altos_Bajos,
    Octeto,
    Rojo_Negro,
    Fila,
    Cuadrado,
    Linea,
    Medio
}