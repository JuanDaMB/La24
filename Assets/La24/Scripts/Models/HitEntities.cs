using System;

namespace IndieLevelStudio.Networking.Models
{
	[Serializable]
	public class HitRequest
	{
		public int deno;
		public int id;
	}

	[Serializable]
	public class HitResponse : BetResultData
	{
		public int balance;
		public int bonusRestricted;
		public int bonusNonrestricted;
	}
}