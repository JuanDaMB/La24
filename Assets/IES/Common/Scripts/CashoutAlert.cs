using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashoutAlert : MonoBehaviour
{
	[SerializeField]
	private Text MainMessage;

	[SerializeField]
	private Button yes;

	public void SetTextValue(int money, int restricted)
	{
		MainMessage.text = "Dinero:\n";
		if (money > 0)
		{
			MainMessage.text += "$" + money.ToString("N0") + "\n";
			yes.interactable = true;
		}
		else
		{
			MainMessage.text += "--\n";
			yes.interactable = false;
		}
		MainMessage.text += "Bono Restringido:\n";
		if (restricted > 0)
		{
			MainMessage.text += "$" + restricted.ToString("N0") + "\n";
		}
		else
		{
			MainMessage.text += "--\n";
		}
		MainMessage.text += "¿Retirar Dinero?";
	}
}
