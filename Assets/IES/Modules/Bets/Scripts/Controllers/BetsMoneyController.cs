using System;
using System.Collections.Generic;
using IndieLevelStudio.BoardModule.Controllers;
using IndieLevelStudio.BoardModule.UX;
using IndieLevelStudio.Common;
using UnityEngine;
using UnityEngine.UI;

namespace IndieLevelStudio.BetsModule.Controllers
{
	public class BetsMoneyController : MonoBehaviour
	{
		public BetsMoneyController Instance
		{
			get { return this; }
		}
		public event Action<DenomType> OnDenominationTypeChanged;

		public event Action<Dictionary<Bet, int>> OnBetsFinished;

		public event Action OnPlayerStartBets;

		[SerializeField]
		private Button buttonPlay;

		[SerializeField]
		private BoardController board;

		[SerializeField]
		private Text monetInHeader;

		[SerializeField]
		public DenominationsController denoms;

		[SerializeField]
		private DenomsChanger coinChanger;

		[SerializeField]
		public MoneyHandler economyHandler;

		[SerializeField]
		private CountdownTimer timer;

		public bool IsBetsOpened {
			get { return board.IsBetsOpened; }
		}

		private bool isStartingGame;

		private void Awake ()
		{
			buttonPlay.onClick.AddListener (PlayImmediatly);
			buttonPlay.interactable = false;

			denoms.OnBetChanged += Denoms_OnBetChanged;
			coinChanger.OnDemonimationChanged += DenominationTypeChanged;

			board.OnCollecttedMoney += Board_OnCollecttedMoney;
			board.OnLastBetRemoved += Board_OnLastBetRemoved;
			board.OnBetPlaced += Board_OnBetPlaced;
			board.OnIndividualBetRemoved += Board_OnIndividualBetRemoved;
			board.OnIndividualBetPlaced += Board_OnIndividualBetPlaced;
			board.OnBetsFinished += Board_OnBetsFinished;
			board.AllBetsRemoved += AllBetsRemoved;

			board.Setup (false);

			ModulesGlobalObjects.LastBetRegister = new Dictionary<Bet, int> ();
		}

		private void Denoms_OnBetChanged (BetInfo betInfo)
		{
			board.BetChanged (betInfo);

			economyHandler.UserMoney = ModulesGlobalObjects.UserMoney;
			economyHandler.UserBet = ModulesGlobalObjects.UserBet;
			economyHandler.Gain = ModulesGlobalObjects.UserGain;
		}

		public void AllBetsRemoved()
		{
			board.OpenBets(false);
			PlayImmediatly();
		}

		public void ClearBets ()
		{
			board.RemoveAllBets ();
		}

		public void ClearBoardAnimated (out float delay)
		{
			board.ClearBoardAnimated (out delay);
		}

		private void OnEnable ()
		{
			board.BetChanged (denoms.CurrentBetInfo);
		}

		private void Board_OnBetsFinished (Dictionary<Bet, int> obj)
		{
			timer.Unable ();

			OnBetsFinished?.Invoke (obj);
		}

		private void Board_OnBetPlaced (BetInfo betInfo)
		{
			if (!board.IsBetsOpened)
				OpenBets ();

			economyHandler.UserMoney -= betInfo.amount * ModulesGlobalObjects.CurrentDenomination;
			economyHandler.UserBet += betInfo.amount * ModulesGlobalObjects.CurrentDenomination;
		}

		private void Board_OnIndividualBetPlaced (int elementValue)
		{
			if (!board.IsBetsOpened)
				OpenBets ();

			economyHandler.UserMoney -= elementValue * ModulesGlobalObjects.CurrentDenomination;
			economyHandler.UserBet += elementValue * ModulesGlobalObjects.CurrentDenomination;
		}

		private void Board_OnIndividualBetRemoved (int elementValue)
		{
			economyHandler.UserMoney += elementValue * ModulesGlobalObjects.CurrentDenomination;
			economyHandler.UserBet -= elementValue * ModulesGlobalObjects.CurrentDenomination;
		}

		private void Board_OnLastBetRemoved (BetInfo lastBetInfo)
		{
			economyHandler.UserMoney += lastBetInfo.amount * ModulesGlobalObjects.CurrentDenomination;
			economyHandler.UserBet -= lastBetInfo.amount * ModulesGlobalObjects.CurrentDenomination;
		}

		private void Board_OnCollecttedMoney ()
		{
			economyHandler.UpdateMoney (ModulesGlobalObjects.UserBet);
			economyHandler.UserBet = 0;
		}

		private void DenominationTypeChanged (DenomType denomType)
		{
			monetInHeader.text = denomType == DenomType.Money? "Dinero:" : "Creditos:";
			ModulesGlobalObjects.IsMoney = denomType == DenomType.Money;

			board.UpdateTokensValue ();

			OnDenominationTypeChanged?.Invoke (denomType);
		}

		public void PlayImmediatly ()
		{
			timer.Unable ();
			TimeToPlay ();
		}

		private void OpenBets ()
		{
			int progresive = UnityEngine.Random.Range (10000, 100000);

			economyHandler.ProgresivePrize = progresive;
			economyHandler.MisteriousPrize = progresive * 3;

			buttonPlay.interactable = true;

			denoms.InteractableDropdown (false);
			board.OpenBets(true);

			timer.SetTimer (XMLManager.BetsTime, TimeToPlay);
			PlaySound.audios.PlayAmbientance ("Clock", 1, (XMLManager.BetsTime * 0.5f), true);

			if (OnPlayerStartBets != null)
				OnPlayerStartBets ();
		}

		private void TimeToPlay ()
		{
			board.OpenBets(false);
			PlaySound.audios.StopAmbiences ();
			board.BetsFinished ();
			
			buttonPlay.interactable = false;
			denoms.InteractableDropdown (true);
		}
	}
}