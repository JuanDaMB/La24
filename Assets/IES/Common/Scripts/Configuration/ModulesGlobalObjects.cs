using System.Collections.Generic;
using IndieLevelStudio.BetsModule.Controllers;
using IndieLevelStudio.BoardModule.UX;

public sealed class ModulesGlobalObjects
{
	//User data attributes
	public static int UserNumberBets = 0;

	public static bool IsPromocional = true;
	public static bool PromocionalOffline = false;
	public static bool ButtonRemoveToken = true;
	public static bool ClickRemoveToken = true;
	public static bool TokenStore = false;
	public static bool ModeMobile = false;
	public static bool DebugCreditStore = false;
	public static bool IsMusicActive = true;
	public static bool IsAudioActive = true;
	public static bool IsModeAdmin = false;
	public static Message UserMessage;

	//Money attributes
	public static string Currency;

	public static int UserMoney;
	public static int UserBet;
	public static float UserGain;

	public static int MisteriousPrize;
	public static int ProgresivePrize;

	public static int CurrentDenomination = 1;

	public static bool IsMoney = true;

	public static Dictionary<Bet, int> LastBetRegister;

	public static DenomType DenominationType = DenomType.Money;
}