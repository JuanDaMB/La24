using System;
using System.Collections.Generic;

namespace IndieLevelStudio.Networking.Models
{
	[Serializable]
	public class ConfigRequest
	{
	}

	[Serializable]
	public class ConfigResponse
	{
		public int minBet;
		public int maxBet;
		public float betTime;
		public List<int> deno;
		public List<int> coins;
		public bool double_up_enable;
		public RecoveryResponse recovered;

	}

	[Serializable]
	public class RecoveryResponse
	{
		public List<BetRecoveryData> game;
		public int bonusNonrestricted;
		public int balance;
		public int totalBet;
		public bool canMiniGame;
		public int bonusRestricted;
		public int userDeno;
		public int gnaResult;
		public string last_request;
		public int gain;
		public int idPlaysession;
	}

	[Serializable]
	public class BetRecoveryData : BetResultData
	{
		public int amount;
		public string value;
		public string type;
	}
}