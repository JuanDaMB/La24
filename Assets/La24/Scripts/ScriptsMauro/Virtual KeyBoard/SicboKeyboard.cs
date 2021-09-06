using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SicboKeyboard : MonoBehaviour
{
	public static SicboKeyboard Instance;

	private Action<string> OnCompleteWriteText;

	public Text showText;

	private string text;
	private string passwordText;

	private bool isPassword;
	private bool isDateFormat;

	public void Setup()
	{
		Instance = this;

		List<KeyboardKey> keys = new List<KeyboardKey>(transform.GetComponentsInChildren<KeyboardKey>());
		keys.ForEach(k => k.Setup(AddValue));
	}

	public void ShowKeyboard(Action<string> onCompleteWriteText, string baseText = "", bool _isPassword = false, bool _isDateFormat = false)
	{
		OnCompleteWriteText = onCompleteWriteText;
		gameObject.SetActive(true);

		text = baseText;
		passwordText = string.Empty;
		showText.text = text;

		isPassword = _isPassword;
		isDateFormat = _isDateFormat;

		if (_isPassword)
			PutPassword(text.Length);
		else
			showText.text = baseText;
	}

	private void PutPassword(int passwordSize)
	{
		for (int i = 0; i < passwordSize; i++)
		{
			passwordText += "*";
			showText.text = passwordText;
		}
	}

	public void HideKeyboard()
	{
		gameObject.SetActive(false);
	}

	public void AddValue(string value)
	{
		if (isDateFormat)
		{
			if (text.Length < 10)
			{
				if (text.Length == 2 || text.Length == 5)
				{
					text += "/" + value;
				}
				else
				{
					text += value;
				}
			}
		}
		else
		{
			text += value;
		}

		if (!isPassword)
			showText.text = text;
		else
		{
			passwordText += "*";
			showText.text = passwordText;
		}
	}

	public void DeleteLastValue()
	{
		if (text.Length > 0)
		{
			text = text.Remove(text.Length - 1);
			if (isPassword)
			{
				passwordText = passwordText.Remove(passwordText.Length - 1);
			}
		}

		if (!isPassword)
			showText.text = text;
		else
			showText.text = passwordText;
	}

	public void OnAccept()
	{
		if (isDateFormat)
		{
			Debug.Log(text.Length);
			if (!string.IsNullOrEmpty(text) && text.Length > 0 && text.Length == 10)
			{
				if (CheckDateFormat(text))
				{
					if (OnCompleteWriteText != null)
						OnCompleteWriteText(showText.text);

					HideKeyboard();
				}
			}
			else
				Alert.Instance.Show("Alerta", "La fecha esta mal escrita");
		}
		else
		{
			if (OnCompleteWriteText != null)
				OnCompleteWriteText(showText.text);

			HideKeyboard();
		}
	}

	private bool CheckDateFormat(string text)
	{
		Debug.Log(DateTime.Now);

		bool isPerfectDate = false;
		bool isDayPerfect = false;
		bool isMonthPerfect = false;
		bool isYearPerfect = false;

		string[] textArray = null;
		int day = 0;
		int month = 0;
		int year = 0;

		try
		{
			textArray = text.Split('/');
			Debug.Log("Dia: " + textArray[0] + " Mes: " + textArray[1] + " Año: " + textArray[2]);

			day = int.Parse(textArray[0]);
			month = int.Parse(textArray[1]);
			year = int.Parse(textArray[2]);
		}
		catch (Exception)
		{
			Alert.Instance.Show("Alerta", "El formato ingresado de fecha no es correcto");
			return isPerfectDate;
		}

		if (day > 0 && day <= 31)
		{
			isDayPerfect = true;
		}
		else
		{
			isPerfectDate = false;
			Alert.Instance.Show("Alerta", "El valor del dia es incorrecto");
		}

		if (isDayPerfect)
		{
			if (month > 0 && month <= 12)
			{
				isMonthPerfect = true;
			}
			else
			{
				isPerfectDate = false;
				Alert.Instance.Show("Alerta", "El valor del mes es incorrecto");
			}
		}

		if (isMonthPerfect)
		{
			if (year > 1916 && year <= 3000)
			{
				isYearPerfect = true;
			}
			else
			{
				isPerfectDate = false;
				Alert.Instance.Show("Alerta", "El valor del año es incorrecto");
			}
		}

		if (isDayPerfect && isMonthPerfect && isYearPerfect)
		{
			isPerfectDate = true;
		}
		return isPerfectDate;
	}

	private void OnDestroy()
	{
		Instance = null;
	}
}