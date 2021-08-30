using System;
using UnityEngine;
using UnityEngine.UI;

namespace IndieLevelStudio.BetsModule.Controllers
{
	public enum DenomType
	{
		Money,
		Credits,
	}

	public class DenomsChanger : MonoBehaviour
	{
		public event Action<DenomType> OnDemonimationChanged;

		[SerializeField]
		private Sprite iconMoney;

		[SerializeField]
		private Sprite iconCredits;

		[SerializeField]
		private Image imageDenom;

		[SerializeField]
		private Text maxBetText;

		private bool playPrimary;

		private void Start ()
		{
			GetComponent<Button> ().onClick.AddListener (() => {
				ModulesGlobalObjects.DenominationType = ModulesGlobalObjects.DenominationType == DenomType.Credits ? DenomType.Money : DenomType.Credits;
				ChangeDenomination ();
			});
			ChangeDenomination ();
		}

		private void ChangeDenomination ()
		{
			if (playPrimary)
				PlaySound.audios.PlayFX ("Monedas Ganadoras", 0.5f);

			imageDenom.sprite = ModulesGlobalObjects.DenominationType == DenomType.Credits ? iconCredits : iconMoney;
			TriggerDenominationChanged (ModulesGlobalObjects.DenominationType);

			SetMaxBetValue ();

			playPrimary = true;
		}

		public void TriggerDenominationChanged (DenomType type)
		{
			if (OnDemonimationChanged != null)
				OnDemonimationChanged (type);
		}

		public void SetMaxBetValue ()
		{
			if (ModulesGlobalObjects.IsMoney) {
				maxBetText.text = "$" + GlobalObjects.MaxBet.ToString ("N0");
			} else {
				maxBetText.text = (GlobalObjects.MaxBet / ModulesGlobalObjects.CurrentDenomination).ToString ();
			}
		}
	}
}