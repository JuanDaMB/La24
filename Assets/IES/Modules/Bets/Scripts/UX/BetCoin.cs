using UnityEngine;
using UnityEngine.UI;

namespace IndieLevelStudio.BetsModule.UX
{
	[RequireComponent(typeof(Button))]
	public class BetCoin : MonoBehaviour
	{
		[SerializeField]
		[Range(1, 1000)]
		public int multiplier;

		[SerializeField]
		public Text denomValue;

		[SerializeField]
		public Text multiplierValue;

		[SerializeField]
		private Image icon;

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

		public Sprite Icon
		{
			get { return icon.sprite; }
		}

		public int MoneyBetAmount
		{
			get;
			private set;
		}

		private void Start()
		{
			multiplierValue.text = multiplier.ToString();
		}

		public void SetDenomination(int denomAmount)
		{
			MoneyBetAmount = multiplier * denomAmount;
			denomValue.text = "$"+MoneyBetAmount.ToString("N0");
			
		}
	}
}