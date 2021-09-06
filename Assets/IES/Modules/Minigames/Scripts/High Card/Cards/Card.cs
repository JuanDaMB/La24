using System;
using IndieLevelStudio.BlackJack.Settings;
using UnityEngine;

namespace IndieLevelStudio.BlackJack.Cards
{
	[Serializable]
	public class Card
	{
		public int objectId;
		public string id;
		public int value;

		public CardColor color;
		public CardSymbol symbol;

		public Sprite icon;

		public int GetPriority()
		{
			return GameSettings.CardIdPriorities.IndexOf(id);
		}
	}
}