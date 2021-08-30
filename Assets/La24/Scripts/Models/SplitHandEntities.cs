using System;
using System.Collections.Generic;

namespace IndieLevelStudio.Networking.Models
{
	[Serializable]
	public class SplitHandRequest : HitRequest
	{
	}

	[Serializable]
	public class SplitHandResponse
	{
		public int balance;
		public int bonusRestricted;
		public int bonusNonrestricted;

		public List<BetResultData> game;
	}
}