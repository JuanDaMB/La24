using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
	[SerializeField]
	private string soundName;

	private void Start()
	{
		GetComponent<Button>().onClick.AddListener(OnClick);
	}

	private void OnClick()
	{
		PlaySound.audios.PlayFX(soundName);
	}
}