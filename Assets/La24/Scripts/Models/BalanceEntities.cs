using System;

namespace IndieLevelStudio.Networking.Models
{
	[Serializable]
	public class BalanceRequest
	{
	}

	[Serializable]
	public class BalanceResponse
	{
		public string currency;
		public int balance;
		public int bonusRestricted;
		public int bonusNonrestricted;
	}
}