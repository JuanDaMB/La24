using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class InputUseKeyboard : MonoBehaviour, ISelectHandler
{
	private InputField input;

	[SerializeField]
	private bool isDateFormat;

	private void Start()
	{
		input = GetComponent<InputField>();
	}

	public void OnSelect(BaseEventData eventData)
	{
		SicboKeyboard.Instance.ShowKeyboard((val) =>
		{
			input.text = val;
		}, input.text, input.contentType == InputField.ContentType.Password, isDateFormat);
	}
}