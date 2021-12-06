using System;
using IndieLevelStudio.Networking.Models;
using UnityEngine;

public static class GlobalObjects
{
	public static bool IsModeAdmin = false;
	public static bool isMoney = false;
	public static Message UserMessage;

	public static string BackendToken;
	public static string gpUser;
	public static string system;
	public static string sysOperator;
	public static string player;
	public static string token;
	public static string game;
	public static string miniGame;
	
	private static int _userMoney;
	private static int _userMoneyReal;
	private static int _userBet;
	private static float _userGain;
	
	public static int Coin;
	public static int Deno;
	
	public static int MinBet;
	public static int MaxBet;

	public static int idPlaysession = 0;
	public static int idPlaysessionDouble = 0;

	public static PrizesTableResponse PrizesData;

	public static bool IsWebGL;
	public static bool IsCashOutMode;
	public static bool IsRecoveryMode = false;
	public static RecoveryResponse RecoveryInfo;

	public static int TotalGamesToRestart;
	public static Action<int> UserBetChanged;
	public static Action<int> UserMoneyChanged;
	public static Action<float> UserGainChanged;
	public static Action<int, string> BetResult;

	public static bool IsMoney
	{
		get => isMoney;
		set
		{
			isMoney = value;
			UserGainChanged?.Invoke(_userGain);
			UserMoneyChanged?.Invoke(_userMoney);
			UserBetChanged?.Invoke(_userBet);
		}
	}

	public static GameState State;
	public static GameState PreviousState;
	public static float UserGain
	{
		get => _userGain;
		set
		{
			_userGain = value;
			UserGainChanged?.Invoke(_userGain);
		}
	}

	public static int UserMoney
	{
		get => _userMoney;
		set
		{
			_userMoney = value;
			UserMoneyChanged?.Invoke(_userMoney);
		}
	}

	public static int UserBet
	{
		get => _userBet;
		set
		{
			_userBet = value;
			UserBetChanged?.Invoke(_userBet);
		}
	}

	public static int UserMoneyReal
	{
		get => _userMoneyReal;
		set
		{
			_userMoneyReal = value;
			UserMoney = value;
		}
	}

	public static void SaveTransactionAttributes<T> (GenericTransaction<T> response) where T : class
	{
		gpUser = response.gpUser;
		system = response.system;
		sysOperator = response.sysOperator;
		player = response.player;
		token = response.token;
		game = response.game;
	}
}

public enum GameState
{
	Bet,
	Playing,
	Pause,
	ShowingResult,
	Idle,
	Instructions,
	Cashout
}
