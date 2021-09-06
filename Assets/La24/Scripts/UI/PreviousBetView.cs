using UnityEngine;
using UnityEngine.UI;

public class PreviousBetView : MonoBehaviour
{
	[SerializeField]
	private Image image;

	[SerializeField]
	private Text text;

	public void Show(Sprite icon, string value)
	{
		image.sprite = icon;
		text.text = value;
	}
}