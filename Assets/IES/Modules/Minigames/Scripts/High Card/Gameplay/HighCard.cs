using System;
using System.Collections.Generic;
using IndieLevelStudio.BlackJack.Cards;
using IndieLevelStudio.BlackJack.Gameplay;
using IndieLevelStudio.Common;
using IndieLevelStudio.Networking;
using IndieLevelStudio.Networking.Models;
using UnityEngine;
using UnityEngine.UI;

namespace IndieLevelStudio.IES.SlotMinigames.HighCard
{
	public class HighCard : MinigameManager
	{
		public override event Action OnStart;

		public override event Action<bool, bool> OnFinish;

		[SerializeField]
		private Sprite headCrupierIcon;

		[SerializeField]
		private Sprite headPlayerIcon;

		[SerializeField]
		private Sprite backCard;

		[SerializeField]
		private GameDeck Deck;

		[SerializeField]
		private List<UICard> cards;

		private UICard crupierCard;

		private UICard chosenCard;

		[SerializeField]
		private Text resultText;

		private bool isPlaying;

		private bool configured;

		private bool hasWinned;

		private bool canDoubleUp;

		private int doubleCount;

		public override bool IsPlaying {
			get { return isPlaying; }
		}

		public float Gain {
			get;
			set;
		}

		public void Setup ()
		{
			Deck.Setup ();
			cards.ForEach (card => {
				card.PassBackCard (backCard);
				card.OnChosenCard += ChosenCard;
			});

			configured = true;
		}

		public override void Show ()
		{
			GenericTransaction<GetDoubleRequest> request = new GenericTransaction<GetDoubleRequest> ();
			request.msgName = "getDoubleup";
			request.game = GlobalObjects.miniGame;
			request.msgDescrip = new GetDoubleRequest ();

			request.msgDescrip.contDoubleup = doubleCount;
			request.msgDescrip.gameHost = GlobalObjects.game;
			request.msgDescrip.gain = Gain;

			string json = JsonUtility.ToJson (request);
			WebServiceManager.Instance.SendJsonData<GenericTransaction<GetDoubleResponse>> (XMLManager.UrlGeneric, json, "authorization", GlobalObjects.BackendToken, GetDoubleResponse, TransactionFailed);

			isPlaying = true;

			GameplayManager.Instance.OnButtonPressed += PhysicButtonPressed;
		}

		private void PhysicButtonPressed (int btnIndex)
		{
			switch (btnIndex) {
			case 4:
				if (cards [1].Button.interactable)
					cards [1].Button.onClick.Invoke ();
				break;
			case 5:
				if (cards [2].Button.interactable)
					cards [2].Button.onClick.Invoke ();
				break;
			case 6:
				if (cards [3].Button.interactable)
					cards [3].Button.onClick.Invoke ();
				break;
			case 7:
				if (cards [4].Button.interactable)
					cards [4].Button.onClick.Invoke ();
				break;
			}
		}

		private void GetDoubleResponse (GenericTransaction<GetDoubleResponse> response)
		{
			if (!configured)
				Setup ();

			Deck.ResetDeck (false);

			GlobalObjects.idPlaysessionDouble = response.msgDescrip.idPlaysession;

			Card card;
			bool notHideCard = true;
			resultText.text = string.Empty;

			foreach (UICard currentCard in cards) {
				card = Deck.GetGNACardImmediatly (response.msgDescrip.dealerCard);
				currentCard.SetCard (card, notHideCard);
				currentCard.gameObject.SetActive (true);
				currentCard.Button.interactable = !notHideCard;
				currentCard.SetHeader (null, false);

				if (notHideCard) {
					crupierCard = currentCard;
					crupierCard.SetHeader (headCrupierIcon, true);
					notHideCard = false;

					PlaySound.audios.PlayFX ("Cartas Esfuman", 0.25f);
				}
			}
			if (OnStart != null)
				OnStart ();

			base.Show ();
		}

		private void ChosenCard (UICard chosenCard)
		{
			this.chosenCard = chosenCard;
			cards.ForEach (c => c.Button.interactable = false);

			GenericTransaction<ValidateDoubleRequest> request = new GenericTransaction<ValidateDoubleRequest> ();
			request.msgName = "validateDoubleup";
			request.game = GlobalObjects.miniGame;
			request.msgDescrip = new ValidateDoubleRequest ();

			request.msgDescrip.holdButton = cards.IndexOf (chosenCard) + 1;
			request.msgDescrip.gameHost = GlobalObjects.game;

			string json = JsonUtility.ToJson (request);
			WebServiceManager.Instance.SendJsonData<GenericTransaction<ValidateDoubleResponse>> (XMLManager.UrlGeneric, json, "authorization", GlobalObjects.BackendToken, ValidateDoubleResponse, TransactionFailed);
		}

		private void ValidateDoubleResponse (GenericTransaction<ValidateDoubleResponse> response)
		{
			int index = 0;
			cards.ForEach (card => {
				card.Button.interactable = false;
				card.SetCard (Deck.GetGNACardImmediatly (response.msgDescrip.cards [index]));

				index++;
			});
			chosenCard.SetHeader (headPlayerIcon, true);
			resultText.text = response.msgDescrip.win ? "¡Felicitaciones! Has duplicado tu ganancia" : "Has perdido tu ganancia";

			PlaySound.audios.PlayFX ("Carta_Voltea_01");

			hasWinned = response.msgDescrip.win;
			canDoubleUp = response.msgDescrip.canDoubleup;

			if (hasWinned) {
				doubleCount++;
				PlaySound.audios.PlayFX ("Ganador", 1, 0.5f);
			} else {
				doubleCount = 0;
				PlaySound.audios.PlayFX ("Perdio", 1, 0.5f);
			}
			EndMinigame ();
		}

		private void EndMinigame ()
		{
			GenericTransaction<EndMinigameRequest> request = new GenericTransaction<EndMinigameRequest> ();
			request.msgName = "endGame";
			request.game = GlobalObjects.miniGame;
			request.msgDescrip = new EndMinigameRequest ();

			request.msgDescrip.gameHost = GlobalObjects.game;

			string json = JsonUtility.ToJson (request);
			WebServiceManager.Instance.SendJsonData<GenericTransaction<EndGameResponse>> (XMLManager.UrlGeneric, json, "authorization", GlobalObjects.BackendToken, EndDoubleResponse, TransactionFailed);
		}

		private void EndDoubleResponse (GenericTransaction<EndGameResponse> response)
		{
			Invoke ("Hide", 1f);
			StartCoroutine (ScreenshotRecorder.SaveScreenshot (XMLManager.ScreenshotsDirectory, 0.3f, null, true));
		}

		public override void Hide ()
		{
			isPlaying = false;
			GameplayManager.Instance.OnButtonPressed -= PhysicButtonPressed;

			base.Hide ();
			if (OnFinish != null)
				OnFinish (hasWinned, canDoubleUp);
		}

		private void TransactionFailed (string errorMessage)
		{
			Alert.Instance.Show ("Transaction Failed", errorMessage);
		}
	}
}