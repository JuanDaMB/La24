using System;
using System.Collections.Generic;
using IndieLevelStudio.BlackJack.Cards;
using IndieLevelStudio.BlackJack.Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IndieLevelStudio.BlackJack.Gameplay
{
	public class GameDeck : MonoBehaviour
	{
		[SerializeField]
		private GameSettings genericDeck;

		[SerializeField]
		private Transform cardsDeck;

		[SerializeField]
		private Transform endPoint;

		[SerializeField]
		[Range(0, 1)]
		public float moveCardTime = 0.15f;

		private Dictionary<Card, SpriteRenderer> cardViews;

		private List<SpriteRenderer> removedCards;

		public List<Card> Deck
		{
			get; private set;
		}

		public void Setup()
		{
			removedCards = new List<SpriteRenderer>();

			Deck = genericDeck.LoadDeck();
			CreateGameDeck();
		}

		private void CreateGameDeck()
		{
			cardViews = new Dictionary<Card, SpriteRenderer>(Deck.Count);

			string viewName;
			SpriteRenderer viewCard;

			int count = 0;
			Deck.ForEach(card =>
			{
				viewName = string.Format("{0} - {1} {2}", card.id, card.color, card.symbol);
				viewCard = new GameObject(viewName).AddComponent<SpriteRenderer>();
				viewCard.transform.SetParent(cardsDeck, true);
				viewCard.transform.localPosition = Vector3.forward * (0.01f * count);
				viewCard.transform.localScale = Vector3.one;
				viewCard.sprite = card.icon;
				viewCard.enabled = false;

				cardViews[card] = viewCard;
				count++;
			});
		}

		public void GetRandomCard(Vector3 position, Action<Card> onComplete)
		{
			Card card = Deck[Random.Range(0, Deck.Count)];
			AnimateAndRemoveCardFromDeck(card, position, onComplete);
		}

		public Card GetGNACardImmediatly(int cardId)
		{
			Card card = Deck.Find(c => c.objectId == cardId);
			if (card != null)
				return card;
			else
			{
				Debug.LogWarning("Card " + cardId + " does not exist in deck");
				return GetFirstCardImmediatly();
			}
		}

		public void GetFirstCard(Vector3 position, Action<Card> onComplete)
		{
			Card card = Deck[0];
			AnimateAndRemoveCardFromDeck(card, position, onComplete);
		}

		public Card GetFirstCardImmediatly()
		{
			Card card = Deck[0];
			RemoveCardFromDeck(card);

			return card;
		}

		private void AnimateAndRemoveCardFromDeck(Card card, Vector3 position, Action<Card> onComplete)
		{
			SpriteRenderer rend = null;
			if (cardViews.TryGetValue(card, out rend))
			{
				rend.enabled = true;
				LeanTween.move(rend.gameObject, position, moveCardTime).setOnComplete(() =>
				{
					cardViews.Remove(card);
					rend.gameObject.SetActive(false);
					removedCards.Add(rend);

					if (onComplete != null)
						onComplete(card);
				});
				Deck.Remove(card);
			}
			else
				Debug.LogWarning("Aqui no esta removiendo nada!");
		}

		private void RemoveCardFromDeck(Card card)
		{
			SpriteRenderer rend = null;
			if (cardViews.TryGetValue(card, out rend))
			{
				cardViews.Remove(card);
				rend.gameObject.SetActive(false);
				removedCards.Add(rend);
				Deck.Remove(card);
			}
			else
				Debug.LogWarning("Aqui no esta removiendo nada!");
		}

		public void ResetDeck(bool animated, Action onComplete = null)
		{
			if (animated)
			{
				removedCards.ForEach(card =>
				{
					card.gameObject.SetActive(true);
					LeanTween.move(card.gameObject, endPoint.position, moveCardTime).setOnComplete(() =>
					{
						card.gameObject.SetActive(false);
						removedCards.Remove(card);

						if (removedCards.Count == 0)
						{
							Resetteo();
							if (onComplete != null)
								onComplete();
						}
					});
				});
			}
			else
				Resetteo();
		}

		private void Resetteo()
		{
			Clear();
			Setup();
		}

		private void Clear()
		{
			foreach (SpriteRenderer rend in cardViews.Values)
				Destroy(rend.gameObject);
			cardViews.Clear();
			removedCards.Clear();
		}
	}
}