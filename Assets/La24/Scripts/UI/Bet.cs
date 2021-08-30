using System.Collections.Generic;
using IndieLevelStudio.BetsModule.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace IndieLevelStudio.BoardModule.UX
{
	[RequireComponent(typeof(Button))]
	public class Bet : MonoBehaviour
	{
		public string DataValue;

		public List<string> numbers;

		[HideInInspector]
		public List<BetInfo> betsInBet;

		[SerializeField]
		public bool isFromColor;

		[SerializeField]
		public NumberColor color;

		[HideInInspector]
		public float amount;

		[HideInInspector]
		public Button button;

		public Reward reward;

		private Token token;

		[SerializeField]
		[Range(1, 8)]
		private int rewardType;

		public float MaxBet
		{
			get; private set;
		}

		public string RewardTypeText
		{
			get; private set;
		}

		public int RewardType
		{
			get { return rewardType; }
		}

		public Token Token
		{
			get { return token; }
		}

		public void Setup()
		{
			button = GetComponent<Button>();
			betsInBet = new List<BetInfo>();

			CalculateNumbers();
		}

		private void CalculateNumbers()
		{
			switch (numbers.Count)
			{
				case 1:
					rewardType = 3;
					break;

				case 2:
					rewardType = 4;
					break;

				case 4:
					rewardType = 2;
					break;
			}
		}

		public void SetBet(Token token, Reward reward)
		{
			this.token = token;
			this.reward = reward;

			token.SetTextAsString("Premio " + rewardType);
			token.transform.position = transform.position;
		}

		public void SetMoneyToken(Token token)
		{
			this.token = token;
			token.transform.position = transform.position;
		}

		public void SetRewardFeatures(string value, int maxBetValue)
		{
			RewardTypeText = value;
			MaxBet = maxBetValue;
		}

		public void RemoveBet()
		{
			if (token != null)
				Destroy(token.gameObject);
			else
				Debug.LogWarning(name + " token is null");

			token = null;
		}
	}
}