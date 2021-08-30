using System;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardKey : MonoBehaviour
{
	[SerializeField]
	private string content;

	[SerializeField]
	private Text text;

	[SerializeField]
	private Button button;

	private void Start()
	{
		SetWordCase(false);
	}

	public void SetWordCase(bool mayus)
	{
		content = mayus ? content.ToUpper() : content.ToLower();
		text.text = content;
	}

	public void Setup(Action<string> onClick)
	{
		text.text = content;
		button.onClick.AddListener(() => { onClick(content); });
	}
}