using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
	[SerializeField]
	private Text place;

	[SerializeField]
	private Text value;

	public RewardData Data
	{
		get; private set;
	}

	public void ShowReward(RewardData data)
	{
		Data = data;

		place.text = data.place + "°";
		value.text = data.GetFormatedValue();
	}
}