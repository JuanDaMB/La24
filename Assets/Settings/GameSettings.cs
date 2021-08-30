using System.Collections.Generic;
using IndieLevelStudio.BlackJack.Cards;
using IndieLevelStudio.Poker.Utils;
using UnityEngine;

namespace IndieLevelStudio.BlackJack.Settings
{
	public enum BuildType
	{
		Demo,
		Money,
		Promotional
	}

	public class GameSettings : ScriptableObject
	{
		[Header("Game Settings")]
		public BuildType type;

		[Header("Build")]
		public bool UseWebServices;

		public bool UseLauncher;

		[Header("Music")]
		[Range(0, 1)]
		public float FadeMusic;

		public string LastGNATexture
		{
			set { lastGNATexture = value; }
			get { return lastGNATexture; }
		}

		[HideInInspector]
		public string lastGNATexture;

		public static List<string> CardIdPriorities = new List<string>() {
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"10",
			"J",
			"Q",
			"K",
			"A"
		};

		public bool IsBetMoney
		{
			get { return type == BuildType.Money; }
		}

		[Space]
		[Header("Game Features")]
		[Space]
		[SerializeField]
		private List<Card> gameCards;

		public List<Card> LoadDeck()
		{
			List<Card> shuffledDeck = new List<Card>(gameCards);
			RandomHelpers.ShuffleArray(shuffledDeck);

			return shuffledDeck;
		}

		private void OnEnable()
		{
			for (int i = 0; i < gameCards.Count; i++)
				gameCards[i].objectId = i + 1;
		}
	}
}