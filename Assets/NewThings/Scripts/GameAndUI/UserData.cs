using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userMoney, userBet, userGain, resultNumber;
    [SerializeField] private Image resultColor;
    [SerializeField] private Sprite rojo, verde, negro;
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
        userMoney.text = "$ "+value.ToString("N0");
    }

    private void SetUserBet(int value)
    {
        userBet.text = "$ "+value.ToString("N0");
    }

    private void SetUserGain(float value)
    {
        userGain.text = "$ "+value.ToString("N0");
    }
}
