using System;
using System.Collections.Generic;

namespace IndieLevelStudio.Networking.Models
{
	[Serializable]
	public class DealerDrawRequest
	{
		public int deno;
	}

	[Serializable]
	public class DealerDrawResponse
	{
		public int balance;
		public int bonusRestricted;
		public int bonusNonrestricted;

		public List<int> dealerHand;
		public List<HandResultData> game;
	}

	[Serializable]
	public class HandResultData
	{
		public int id;
		public int score;
		public int scoreMajor;
		public int win;
	}
}