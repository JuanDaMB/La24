using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyStadistics : MonoBehaviour
{
	[SerializeField]
	private Sprite iconRed;

	[SerializeField]
	private Sprite iconBlack;

	[SerializeField]
	private Sprite iconGreen;

	[SerializeField]
	private RectTransform lastParent;

	[SerializeField]
	private RectTransform coldsParent;

	[SerializeField]
	private RectTransform hotsParent;

	[SerializeField]
	private Button btnHide;

	private Image currNumberView;

	private void Start()
	{
		btnHide.onClick.AddListener(Hide);
	}

	public void Show(List<int> last, List<int> hot, List<int> cold)
	{
		ShowLastNumbers(last);
		ShowHotNumbers(hot);
		ShowColdNumbers(cold);

		gameObject.SetActive(true);
	}

	private void ShowLastNumbers(List<int> last)
	{
		if (last.Count == 0)
			return;

		int index = 0;
		for (int i = last.Count - 1; i >= 0; i--)
		{
			if (index == lastParent.childCount)
				break;

			currNumberView = lastParent.GetChild(index).GetComponent<Image>();
			SetNumber(currNumberView, last[i]);
			index++;
		}
	}

	private void ShowHotNumbers(List<int> hot)
	{
		if (hot.Count == 0)
			return;

		int index = 0;
		for (int i = hot.Count - 1; i >= 0; i--)
		{
			if (index == hotsParent.childCount)
				break;

			currNumberView = hotsParent.GetChild(index).GetComponent<Image>();
			SetNumber(currNumberView, hot[i]);
			index++;
		}
	}

	private void ShowColdNumbers(List<int> cold)
	{
		if (cold.Count == 0)
			return;

		int index = 0;
		for (int i = cold.Count - 1; i >= 0; i--)
		{
			if (index == coldsParent.childCount)
				break;

			currNumberView = coldsParent.GetChild(index).GetComponent<Image>();
			SetNumber(currNumberView, cold[i]);
			index++;
		}
	}

	private void SetNumber(Image currNumberView, int currNumber)
	{
		if (currNumber > 24)
		{
			currNumberView.sprite = iconBlack;
			currNumber -= 24;
		}
		else
			currNumberView.sprite = iconRed;

		if (currNumber == 1 || currNumber == 24)
			currNumberView.sprite = iconGreen;

		currNumberView.transform.GetComponentInChildren<Text>().text = currNumber.ToString();
		currNumberView.enabled = true;
	}

	private void Hide()
	{
		gameObject.SetActive(false);
	}
}