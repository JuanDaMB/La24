using System;
using UnityEngine;

namespace IndieLevelStudio.BetsModule.Controllers
{
	public class MoneyHandler : MonoBehaviour
	{
		public event Action OnMoneyFromZero;

		public event Action<string> OnMoneyValueChanged;

		public event Action<string> OnUserBetValueChanged;

		public event Action<string> OnMisteriousPriceValueChanged;

		public event Action<string> OnProgresivePriceValueChanged;

		public event Action<string> OnGainValueChanged;

		public int UserMoney
		{
			set
			{
				if (ModulesGlobalObjects.UserMoney == 0 && value > 0)
				{
					if (OnMoneyFromZero != null)
						OnMoneyFromZero();
				}
				ModulesGlobalObjects.UserMoney = value;
				string formattedValue = string.Empty;
				
				if (ModulesGlobalObjects.IsMoney)
					formattedValue = "$" + ModulesGlobalObjects.UserMoney.ToString("N0");
				else
					formattedValue = ((ModulesGlobalObjects.UserMoney / ModulesGlobalObjects.CurrentDenomination).ToString());

				if (OnMoneyValueChanged != null)
					OnMoneyValueChanged(formattedValue);
			}
			get { return ModulesGlobalObjects.UserMoney; }
		}

		public int UserBet
		{
			set
			{
				ModulesGlobalObjects.UserBet = value;
				string formattedValue = string.Empty;
				

				if (ModulesGlobalObjects.IsMoney)
					formattedValue = "$" + ModulesGlobalObjects.UserBet.ToString("N0");
				else
					formattedValue = ((ModulesGlobalObjects.UserBet / ModulesGlobalObjects.CurrentDenomination).ToString());

				if (OnUserBetValueChanged != null)
					OnUserBetValueChanged(formattedValue);
			}
			get { return ModulesGlobalObjects.UserBet; }
		}

		public int MisteriousPrize
		{
			set
			{
				ModulesGlobalObjects.MisteriousPrize = value;
				string formattedValue = string.Empty;

				if (ModulesGlobalObjects.IsMoney)
					formattedValue = "$" + ModulesGlobalObjects.MisteriousPrize.ToString("N0");
				else
					formattedValue = ((ModulesGlobalObjects.MisteriousPrize / ModulesGlobalObjects.CurrentDenomination).ToString());

				if (OnMisteriousPriceValueChanged != null)
					OnMisteriousPriceValueChanged(formattedValue);
			}
			get { return ModulesGlobalObjects.MisteriousPrize; }
		}

		public int ProgresivePrize
		{
			set
			{
				ModulesGlobalObjects.ProgresivePrize = value;
				string formattedValue = string.Empty;

				if (ModulesGlobalObjects.IsMoney)
					formattedValue = "$" + ModulesGlobalObjects.ProgresivePrize.ToString("N0");
				else
					formattedValue = ((ModulesGlobalObjects.ProgresivePrize / ModulesGlobalObjects.CurrentDenomination).ToString());

				if (OnProgresivePriceValueChanged != null)
					OnProgresivePriceValueChanged(formattedValue);
			}
			get { return ModulesGlobalObjects.ProgresivePrize; }
		}

		public float Gain
		{
			set
			{
				ModulesGlobalObjects.UserGain = value;
				
				string formattedValue = string.Empty;
				
				if (ModulesGlobalObjects.IsMoney)
					formattedValue = "$" + ModulesGlobalObjects.UserGain .ToString("N0");
				else
					formattedValue = (ModulesGlobalObjects.UserGain / ModulesGlobalObjects.CurrentDenomination).ToString();

				if (OnGainValueChanged != null)
					OnGainValueChanged(formattedValue);
			}
			get { return ModulesGlobalObjects.UserGain; }
		}

		/// <summary>
		/// Method to add or substract a quantity at user money
		/// </summary>
		/// <param name="amount">Quantity to modify user money</param>
		public void UpdateMoney(int amount)
		{
			UserMoney += amount;
		}
	}
}