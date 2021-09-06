using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PreviousBetData
{
	public List<PreviousBet> data;
}

public class Stadistics : MonoBehaviour
{
	public PreviousBetData PreviousBets;

	[SerializeField]
	private Sprite iconRed;

	[SerializeField]
	private Sprite iconBlack;

	[SerializeField]
	private PreviousBetView viewPrefab;

	public string PreviousBetString
	{
		get { return PlayerPrefs.GetString("PREVIOUSS_BETS_JSON", string.Empty); }
		set { PlayerPrefs.SetString("PREVIOUSS_BETS_JSON", value); }
	}

	public void Setup()
	{
		PreviousBets = JsonUtility.FromJson<PreviousBetData>(PreviousBetString);
		ShowPreviousNumbers();
	}

	private void ShowPreviousNumbers()
	{
		if (PreviousBets == null) return;
		foreach (PreviousBet previousBet in PreviousBets.data)
		{
			PreviousBetView view = Instantiate(viewPrefab, transform, false);
			view.Show(previousBet.color == "Red" ? iconRed : iconBlack, previousBet.value);
		}
	}

	public void SaveBet(PreviousBet bet)
	{
		if (PreviousBets == null)
		{
			PreviousBets = new PreviousBetData();
			PreviousBets.data = new List<PreviousBet>();
		}

		PreviousBets.data.Add(bet);
		if (PreviousBets.data.Count > 18)
			PreviousBets.data.RemoveAt(0);

		PreviousBetString = JsonUtility.ToJson(PreviousBets);
	}
}