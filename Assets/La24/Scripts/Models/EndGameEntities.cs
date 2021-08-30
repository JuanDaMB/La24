using System;

namespace IndieLevelStudio.Networking.Models
{
	[Serializable]
	public class EndGameRequest
	{
	}

	[Serializable]
	public class EndMinigameRequest
	{
		public string gameHost;
	}

	[Serializable]
	public class EndGameResponse
	{
		public int balance;
		public int bonusRestricted;
		public int bonusNonrestricted;
	}
}