using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NewThings.Scripts.GameAndUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct BetOrder
{
    private string _key;
    private int _value, _index;
    private BetType _type;

    public BetOrder(string key, int value, int index, BetType type)
    {
        _key = key;
        _value = value;
        _index = index;
        _type = type;
    }

    public string Key => _key;
    public int Index => _index;

    public int Value => _value;

    public BetType betType => _type;
}


public class BetBoard : MonoBehaviour
{
    [SerializeField] private List<BetBoardButton> _buttons;
    [SerializeField] private List<Button> _images;
    [SerializeField] private List<CoinData> coinDatas;

    [SerializeField] private Color _blank, _color;

    private Dictionary<string, (int value, BetType type)> betValues;
    private Dictionary<string, (int value, BetType type)> lastBet;

    private List<BetOrder> betOrder;
    private List<BetOrder> lastBetOrder;

    public Action<bool> onBet;

    private void Start()
    {
        betValues = new Dictionary<string, (int, BetType)>();
        lastBet = new Dictionary<string, (int, BetType)>();
        betOrder = new List<BetOrder>();
        lastBetOrder = new List<BetOrder>();   
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].Initialize(_blank,_color,_images[i], i, SetBetCoin);
        }
    }

    public void ConfigBoard()
    {
        foreach (var t in _buttons)
        {
            t.SetMaxBet();
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

        foreach (BetOrder tuple in lastBetOrder)
        {
            SetBetCoin(tuple.Index,tuple.Key,tuple.Value, tuple.betType, _buttons[tuple.Index].maxBet);
        }
    }
    
    public void ClearBoard()
    {
        GlobalObjects.UserBet = 0;
        GlobalObjects.UserMoney = GlobalObjects.UserMoneyReal - GlobalObjects.UserBet;
        foreach (BetBoardButton button in _buttons)
        {
            button.ClearData();
            button.UnblockConnections();
        }
        betValues.Clear();
        betOrder.Clear();
        onBet?.Invoke(false);
    }

    public void ClearLast()
    {
        if (betOrder.Count <= 0) return;
        
        BetOrder value = betOrder[betOrder.Count-1];
        
        if (!betValues.ContainsKey(value.Key)) return;
        
        betOrder.RemoveAt(betOrder.Count-1);
        betValues[value.Key] = (betValues[value.Key].value-value.Value,betValues[value.Key].type);
        
        GlobalObjects.UserBet -= value.Value * GlobalObjects.Deno;
        GlobalObjects.UserMoney = GlobalObjects.UserMoneyReal - GlobalObjects.UserBet;
        _buttons[value.Index].SetText((betValues[value.Key].value).ToString());
        
        if (betValues[value.Key].Item1 <= 0)
        {
            betValues.Remove(value.Key);
            _buttons[value.Index].SetImage(null, false);
            _buttons[value.Index].UnblockConnections();
        }
        else
        {
            int coin = 0;
            for (int i = betOrder.Count -1; i >= 0 ; i--)
            {
                if (betOrder[i].Key != value.Key) continue;
                coin = betOrder[i].Value;
                break;
            }
            foreach (var data in coinDatas.Where(data => data.Value == coin))
            {
                _buttons[value.Index].SetImage(data.sprite, true);
                break;
            }
        }

        if (betValues.Count <= 0)
        {
            onBet?.Invoke(false);
        }
    }

    private void SetBetCoin(int indexValue, string value, BetType type, int maxBetValue)
    {
        if (HasMoney()) return;
        if (ValidBet()) return;
        
        if (!betValues.ContainsKey(value))
        {
            betValues.Add(value, (0,type));
        }
        var valueTuple = betValues[value];
        if (MaxBet(maxBetValue, valueTuple.value)) return;
        onBet?.Invoke(true);
        valueTuple.value += GlobalObjects.Coin;

        ApplyData(indexValue, valueTuple, GlobalObjects.Coin);

        betValues[value] = valueTuple;
        GlobalObjects.UserBet += GlobalObjects.Coin * GlobalObjects.Deno;
        GlobalObjects.UserMoney = GlobalObjects.UserMoneyReal - GlobalObjects.UserBet;
        betOrder.Add(new BetOrder(value, GlobalObjects.Coin, indexValue, type));
    }

    public void SetEnabled()
    {
        foreach (BetBoardButton button in _buttons)
        {
            button.UnblockConnections();
        }
    }


    private void SetBetCoin(int indexValue, string value, int coinValue, BetType type, int maxBetValue)
    {
        if (HasMoney(coinValue)) return;
        if (ValidBet(coinValue)) return;
        
        if (!betValues.ContainsKey(value))
        {
            betValues.Add(value, (0,type));  
        }
        var valueTuple = betValues[value];
        if (MaxBet(maxBetValue, valueTuple.value, coinValue)) return;
        onBet?.Invoke(true);
        valueTuple.value += coinValue;


        ApplyData(indexValue, valueTuple, coinValue);
        
        betValues[value] = valueTuple;
        GlobalObjects.UserBet += coinValue * GlobalObjects.Deno;
        GlobalObjects.UserMoney = GlobalObjects.UserMoneyReal - GlobalObjects.UserBet;
        betOrder.Add(new BetOrder(value, coinValue, indexValue, type));
    }

    private void ApplyData(int indexValue, (int value, BetType type) valueTuple, int CoinValue)
    {
        bool Assigned = false;

        foreach (CoinData data in coinDatas)
        {
            if (data.Value == CoinValue)
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
        _buttons[indexValue].HasBet();
    }

    private static bool MaxBet(int maxBet, int betOnNumber)
    {
        return (betOnNumber+GlobalObjects.Coin)*GlobalObjects.Deno>maxBet;
    }
    
    private static bool ValidBet()
    {
        return GlobalObjects.UserBet + (GlobalObjects.Coin * GlobalObjects.Deno) > GlobalObjects.MaxBet;
    }

    private static bool HasMoney()
    {
        return GlobalObjects.UserBet + (GlobalObjects.Coin * GlobalObjects.Deno) > GlobalObjects.UserMoneyReal;
    }
    
    private static bool ValidBet(int coin)
    {
        return GlobalObjects.UserBet + (coin * GlobalObjects.Deno) > GlobalObjects.MaxBet;
    }

    private static bool HasMoney(int coin)
    {
        return GlobalObjects.UserBet + (coin * GlobalObjects.Deno) > GlobalObjects.UserMoneyReal;
    }
    private static bool MaxBet(int maxBet, int betOnNumber, int coin)
    {
        return (betOnNumber+coin)*GlobalObjects.Deno>maxBet;
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