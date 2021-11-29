using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonZone : MonoBehaviour
{
    [SerializeField] private Button clearBets;
    [SerializeField] private Button clearLastBet;
    [SerializeField] private Button reDoLastBet;
    [SerializeField] private Button play;
    [SerializeField] private Button cashout;
    [SerializeField] private Button exit;
    [SerializeField] private Button estadisticas;
    
    public void SuscribeClearBets(Action action)
    {
        clearBets.onClick.AddListener(()=>action());
    }
    public void SuscribeclearLastBet(Action action)
    {
        clearLastBet.onClick.AddListener(()=>action());
    }
    public void SuscribeReDoLastBet(Action action)
    {
        reDoLastBet.onClick.AddListener(()=>action());
    }
    public void SuscribePlay(Action action)
    {
        play.onClick.AddListener(()=>action());
    }

    public void SuscribeCashout(Action action)
    {
        cashout.onClick.AddListener(()=>action());
    }

    public void SuscribeExit(Action action)
    {
        exit.onClick.AddListener(()=> action());
    }

    public void SuscribeStats(Action action)
    {
        estadisticas.onClick.AddListener(()=> action());
    }
    
}
