using System;

namespace IndieLevelStudio.Networking.Models
{
	[Serializable]
	public class DuplicateRequest : HitRequest
	{
	}

	[Serializable]
	public class DuplicateResponse : BetResultData
	{
		public int balance;
		public int bonusRestricted;
		public int bonusNonrestricted;
		public int handBet;
	}
}