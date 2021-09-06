using System;
using IndieLevelStudio.BlackJack.Cards;
using UnityEngine;
using UnityEngine.UI;

namespace IndieLevelStudio.BlackJack.Gameplay
{
	[RequireComponent(typeof(Button))]
	[RequireComponent(typeof(Image))]
	public class UICard : MonoBehaviour
	{
		public event Action<UICard> OnChosenCard;

		private Sprite backCard;

		[SerializeField]
		private Image headImage;

		private Image myRenderer;

		private Button button;

		public Button Button
		{
			get
			{
				if (button == null)
					button = GetComponent<Button>();

				return button;
			}
		}

		private Image Renderer
		{
			get
			{
				if (myRenderer == null)
					myRenderer = GetComponent<Image>();

				return myRenderer;
			}
		}

		public void PassBackCard(Sprite backCard)
		{
			this.backCard = backCard;
		}

		private void Awake()
		{
			Button.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			if (OnChosenCard != null)
				OnChosenCard(this);
		}

		public Card Card
		{
			get;
			private set;
		}

		public void SetCard(Card card, bool showCardIcon = true)
		{
			Card = card;
			Renderer.sprite = showCardIcon ? card.icon : backCard;
		}

		public void SetHeader(Sprite icon, bool enabled)
		{
			headImage.sprite = icon;
			headImage.enabled = enabled;
		}
	}
}