using System;
using Random = UnityEngine.Random;

namespace IndieLevelStudio.Util
{
	public class Tools
	{
		public static string NameOf(Action method)
		{
			return method.Method.Name;
		}

		public static bool TakeDesicion(float probabilityYes)
		{
			if (probabilityYes > 100)
				return true;

			float val = Random.Range(0, 101);
			return val >= probabilityYes;
		}
	}
}