using System;
using System.Collections.Generic;

namespace IndieLevelStudio.Networking.Models
{
	[Serializable]
	public class StadisticsRequest
	{
	}

	[Serializable]
	public class StadisticsResponse
	{
		public List<int> last;
		public List<int> hot;
		public List<int> cold;
	}
}