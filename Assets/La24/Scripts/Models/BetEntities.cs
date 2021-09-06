using System;
using System.Collections.Generic;

namespace IndieLevelStudio.Networking.Models
{
	[Serializable]
	public class BetRequest
	{
		public int deno;
		public List<BetGameData> bets;
	}

	[Serializable]
	public class BetGameData
	{
		public int id;
		public float amount;
		public string value;
		public string type;
	}

	[Serializable]
	public class BetResponse
	{
		public int balance;
		public int bonusRestricted;
		public int bonusNonrestricted;
		public bool canMiniGame;
		public float gain;
		public int deno;
		public int gnaResult;
		public List<BetResultData> game;
		public int idPlaysession;
	}

	[Serializable]
	public class BetResultData
	{
		public int id;
		public float win;
	}
}