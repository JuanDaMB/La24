using System;
using System.Collections.Generic;
using IndieLevelStudio.BetsModule.UX;
using UnityEngine;
using UnityEngine.UI;

namespace IndieLevelStudio.BetsModule.Controllers
{
	public class BetInfo
	{
		public int amount;

		public BetInfo (int amount)
		{
			this.amount = amount;
			
		}
	}

	public class DenominationsController : MonoBehaviour
	{
		public event Action<BetInfo> OnBetChanged;

		[SerializeField]
		private Dropdown denomSelector;

		[SerializeField]
		private Sprite denomSprite;

		[SerializeField]
		private List<BetCoin> denominations;

		[HideInInspector]
		public List<BetCoin> denomsInGame;

		private BetCoin currentCoin;

		private bool playFirst;

		public int CurrentDenomination {
			set { ModulesGlobalObjects.CurrentDenomination = value; }
			get { return ModulesGlobalObjects.CurrentDenomination; }
		}

		public BetInfo CurrentBetInfo {
			get;
			private set;
		}

		private void SetupDropdown (List<int> denoms)
		{
			denomSelector.ClearOptions ();

			List<Dropdown.OptionData> options = new List<Dropdown.OptionData> ();
			Dropdown.OptionData option;
			foreach (int denom in denoms) {
				option = new Dropdown.OptionData (denom.ToString ());
				option.image = denomSprite;
				option.text = denom.ToString ();

				options.Add (option);
			}
			denomSelector.AddOptions (options);
			denomSelector.value = 0;

			denomSelector.onValueChanged.Invoke (0);
		}

		public void SetDenominations (List<int> denoms, List<int> coins)
		{
			SetupDropdown (denoms);

			denomsInGame = new List<BetCoin> ();
			for (int i = 0; i < coins.Count; i++) {
				if (i == denominations.Count)
					break;

				BetCoin c = denominations [i];
				c.gameObject.SetActive (true);
				c.multiplier = coins [i];
				c.Button.onClick.AddListener (() => SelectedBet (c));
				denomsInGame.Add (c);
			}

			denomSelector.onValueChanged.AddListener ((index) => {
				CurrentDenomination = int.Parse (denomSelector.options [index].text);
				denomsInGame.ForEach (coin => coin.SetDenomination (CurrentDenomination));

				if (currentCoin != null)
					SelectedBet (currentCoin);
			});
			Initialize ();
		}

		private void Initialize ()
		{
			CurrentDenomination = int.Parse (denomSelector.options [0].text);
			denomsInGame.ForEach (coin => coin.SetDenomination (CurrentDenomination));

			SelectedBet (denomsInGame [denomSelector.value]);
			gameObject.SetActive (true);
		}

		private void SelectedBet (BetCoin coin)
		{
			if (currentCoin != null) {
				LeanTween.cancel (currentCoin.transform.GetChild (0).gameObject);
				LeanTween.moveLocalY (currentCoin.transform.GetChild (0).gameObject, 0, 0);
			}

			currentCoin = coin;
			CurrentBetInfo = new BetInfo (currentCoin.MoneyBetAmount / ModulesGlobalObjects.CurrentDenomination);

			if (OnBetChanged != null)
				OnBetChanged (CurrentBetInfo);
			
			if (playFirst)
				PlaySound.audios.PlayFX ("Boton Fichas");
			
			LeanTween.moveLocalY (currentCoin.transform.GetChild (0).gameObject, 7f, 0.5f).setLoopPingPong ();
			playFirst = true;
		}

		public void InteractableDropdown (bool interactable)
		{
			denomSelector.interactable = interactable;
		}
	}
}