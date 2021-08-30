using System;
using System.Collections.Generic;

namespace IndieLevelStudio.Networking.Models
{
	[Serializable]
	public class ValidateDoubleRequest
	{
		public int holdButton;
		public string gameHost;
	}

	[Serializable]
	public class ValidateDoubleResponse
	{
		public List<int> cards;
		public bool win;
		public int gain;
		public int contDoubleup;
		public bool canDoubleup;
	}
}