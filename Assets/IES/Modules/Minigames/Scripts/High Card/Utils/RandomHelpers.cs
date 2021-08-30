using System.Collections.Generic;
using UnityEngine;

namespace IndieLevelStudio.Poker.Utils
{
	public static class RandomHelpers
	{
		//Shuffles the array sent as parammeter
		public static void ShuffleArray<T>(T[] array)
		{
			int arrSize = array.Length;

			for (int i = 0; i < arrSize; i++)
			{
				int indexToSwap = Random.Range(0, arrSize);

				T elementToSwap = array[i];
				array[i] = array[indexToSwap];
				array[indexToSwap] = elementToSwap;
			}
		}

		public static void ShuffleArray<T>(List<T> array)
		{
			int arrSize = array.Count;

			for (int i = 0; i < arrSize; i++)
			{
				int indexToSwap = Random.Range(0, arrSize);

				T elementToSwap = array[i];
				array[i] = array[indexToSwap];
				array[indexToSwap] = elementToSwap;
			}
		}
	}
}