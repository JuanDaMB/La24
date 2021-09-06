using IndieLevelStudio.Networking.Models;

public sealed class GlobalObjects
{
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

	public static string BackendToken;
	public static string gpUser;
	public static string system;
	public static string sysOperator;
	public static string player;
	public static string token;
	public static string game;
	public static string miniGame;

	public static bool IsLoggedIn;

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