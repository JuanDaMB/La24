using System.Collections.Generic;
using IndieLevelStudio.BoardModule.UX;
using UnityEngine;
using UnityEngine.UI;

public class BetsBoard : UIZone
{
	public override ZoneName Zone
	{
		get { return ZoneName.Bets; }
	}

	[SerializeField]
	private List<Reward> rewards;

	[SerializeField]
	private List<Reward> finalRewards;

	[Header("Board")]
	[SerializeField]
	private Transform container;

	[SerializeField]
	private Token tokenPrefab;

	[SerializeField]
	private Text betsCountText;

	[SerializeField]
	private Button btnPlay;

	private List<Bet> bets;

	private List<Bet> chosenBets;

	private int betCounts;

	private bool CanPlay
	{
		get { return chosenBets.Count == betCounts; }
	}

	private void Start()
	{
		bets = new List<Bet>(container.GetComponentsInChildren<Bet>());
		chosenBets = new List<Bet>();

		PlaySound.audios.PlayFX("SELECCIONA TUS APUESTAS");

		btnPlay.onClick.AddListener(StartGame);
		bets.ForEach(bet =>
		{
			bet.Setup();
			bet.button.onClick.AddListener(() => { OnClickBet(bet); });
		});
		RewardChange();
	}

	private void OnClickBet(Bet bet)
	{
		if (!chosenBets.Contains(bet))
		{
			if (!CanPlay)
			{
				chosenBets.Add(bet);

				PlaySound.audios.PlayFX("Button Sound 3");

				Token t = Instantiate(tokenPrefab, container, false);
				bet.SetBet(t, finalRewards[bet.RewardType - 1]);
			}
		}
		else
		{
			bet.RemoveBet();
			chosenBets.Remove(bet);

			PlaySound.audios.PlayFX("Button Sound 1");
		}
		RewardChange();
	}

	private void RewardChange()
	{
		betsCountText.text = (betCounts - chosenBets.Count).ToString();
		btnPlay.interactable = CanPlay;
	}

	private void StartGame()
	{
		GameplayManager.Instance.ChosenBets = chosenBets;
		GameplayManager.Instance.StartGameplay();

		UIController.Instance.DisableAll();
	}

	public void ShowBets(int betCounts)
	{
		this.betCounts = betCounts;
		UIController.Instance.MoveToZone(ZoneName.Bets);
	}
}