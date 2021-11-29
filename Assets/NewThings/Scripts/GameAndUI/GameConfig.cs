using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameConfig : MonoBehaviour
{
    [SerializeField] private TimeBar _bar;
    [SerializeField] private CoinHandler _coinHandler;
    [SerializeField] private DenominationHandler _denominationHandler;
    [SerializeField] private BetBoard _board;
    [SerializeField] private ButtonZone _buttonZone;
    [SerializeField] private Button skipButton;
    public Bola ball;
    public void Configuracion(Action EndBet, Action<int> DenoChanged, Action<int> CoinChanged, Action<bool> StartBet, Action CashOut, Action QuitGame,Action stats, Action TerminarRecorrido,  UnityAction SkipBallCameraRotation)
    {
        _bar.OnEndTimer += EndBet;
        _denominationHandler.currentDeno += DenoChanged;
        _coinHandler.CoinChange += CoinChanged;
        _board.onBet += StartBet;
        _buttonZone.SuscribeClearBets(_board.ClearBoard);
        _buttonZone.SuscribeclearLastBet(_board.ClearLast);
        _buttonZone.SuscribeReDoLastBet(_board.ReDoBet);
        _buttonZone.SuscribePlay(_bar.EndTime);
        _buttonZone.SuscribeCashout(CashOut);
        _buttonZone.SuscribeExit(QuitGame);
        _buttonZone.SuscribeStats(stats);
        ball.OnCompleteMove += TerminarRecorrido;
        skipButton.onClick.AddListener(SkipBallCameraRotation);
    }

    public void BarConfig(float value)
    {
        _bar.SetMaxTime(value);
    }

    public void SetOnBet(bool onBet)
    {
        _denominationHandler.OnBetState(onBet);
        if (onBet)
        {
            _bar.StartTime();
        }
        else
        {
            _bar.EndTime();
        }
    }
}
