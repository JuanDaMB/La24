using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userMoney, userBet, userGain, resultNumber, symbol1,symbol2,symbol3, moneda, dinero;
    [SerializeField] private Image resultColor;
    [SerializeField] private Sprite rojo, verde, negro;
    [SerializeField] private Image currencyButton;
    [SerializeField] private Sprite currencyAzul, currencyVerde, moneyAzul, moneyVerde;
    [SerializeField] private SceneColor _color;

    public void ChangeColor(SceneColor color)
    {
        _color = color;
        switch (color)
        {
            case SceneColor.Azul:
                currencyButton.sprite = GlobalObjects.IsMoney ? moneyAzul : currencyAzul;
                break;
            case SceneColor.Verde:
                currencyButton.sprite = GlobalObjects.IsMoney ? moneyVerde : currencyVerde;
                break;
        }  
    }

    public void ChangeCurrency()
    {
        GlobalObjects.IsMoney = !GlobalObjects.IsMoney;
        if (GlobalObjects.IsMoney)
        {
            symbol1.text = "$";
            symbol2.text = "$";
            symbol3.text = "$";
            moneda.text = "Dinero";
            dinero.text = "Dinero";
        }
        else
        {
            symbol1.text = "¢";
            symbol2.text = "¢";
            symbol3.text = "¢";
            moneda.text = "Créditos";
            dinero.text = "Créditos";
        }
        ChangeColor(_color);
    }
    private void Awake()
    {
        GlobalObjects.UserBetChanged += SetUserBet;
        GlobalObjects.UserGainChanged += SetUserGain;
        GlobalObjects.UserMoneyChanged += SetUserMoney;
        GlobalObjects.BetResult += SetBetResult;
    }

    private void SetBetResult(int number, string color)
    {
        switch (color)
        {
            case "Red":
                resultColor.sprite = rojo;
                break;
            case "Green":
                resultColor.sprite = verde;
                break;
            case "Black":
                resultColor.sprite = negro;
                break;
        }

        resultNumber.text = number.ToString();
    }

    private void SetUserMoney(int value)
    {
        userMoney.text = GlobalObjects.IsMoney ? value.ToString("N0") : (value/GlobalObjects.Deno).ToString("N0");
    }

    private void SetUserBet(int value)
    {
        userBet.text = GlobalObjects.IsMoney ? value.ToString("N0") : (value/GlobalObjects.Deno).ToString("N0");
    }

    private void SetUserGain(float value)
    {
        userGain.text = GlobalObjects.IsMoney ? value.ToString("N0") : (value/GlobalObjects.Deno).ToString("N0");
    }
}
