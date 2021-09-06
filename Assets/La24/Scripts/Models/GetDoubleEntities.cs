using System;

namespace IndieLevelStudio.Networking.Models
{
	[Serializable]
	public class GetDoubleRequest
	{
		public float gain;
		public int contDoubleup;
		public string gameHost;
		public bool selection;
	}

	[Serializable]
	public class GetDoubleResponse
	{
		public int dealerCard;
		public int idPlaysession;
	}
}