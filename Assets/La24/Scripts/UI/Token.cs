using UnityEngine;
using UnityEngine.UI;

namespace IndieLevelStudio.BoardModule.UX
{
	[RequireComponent(typeof(Image))]
	public class Token : MonoBehaviour
	{
		[SerializeField]
		private Text text;

		[HideInInspector]
		public int value;

		public int Value
		{
			get
			{
				if (ModulesGlobalObjects.IsMoney)
				{
					return value * ModulesGlobalObjects.CurrentDenomination;
				}
				return value;
			}
			set { this.value = value; }
		}


		public void SetTextAsString(string val)
		{
			text.text = val;
		}

		public void SetText(int value)
		{
			this.value = value;
			UpdateValue();
		}

		public void UpdateValue()
		{
			if (ModulesGlobalObjects.IsMoney)
				text.text = "$" + (value * ModulesGlobalObjects.CurrentDenomination).ToString("N0");
			else
				text.text = value.ToString();
		}
	}
}