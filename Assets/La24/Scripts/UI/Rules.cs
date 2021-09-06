using UnityEngine;
using UnityEngine.UI;

public class Rules : UIZone
{
	public override ZoneName Zone
	{
		get { return ZoneName.Rules; }
	}

	[SerializeField]
	private Text textRules;

	[SerializeField]
	private Button buttonYes;

	[SerializeField]
	private Button buttonNo;

	private void Start()
	{
		buttonYes.onClick.AddListener(PressedYes);
		buttonNo.onClick.AddListener(PressedNot);

		SetTextRules();
	}

	private void SetTextRules()
	{
		/*textRules.text = "Autorizo de manera previa, voluntaria, explicita e informada a la empresa " + XMLManager.Operators + " identificada con nit " + XMLManager.Nit +
			", para tratar mis datos de acuerdo a la política de tratamiento de datos personales de la empresa " +
			"y para los fines relacionados a su objeto social, ello acorde a lo establecido en la " +
			"ley estatutaria 1581 de 2012 y el decreto reglamentario 1377 de 2013.";*/
	}

	private void PressedYes()
	{
		/*DataBaseManager.instance.UpdateClient(GlobalObjects.UserMessage.clientes.login, GlobalObjects.UserMessage.clientes.pass, "true", XMLManager.Nit);
		UIController.Instance.MoveToZone(ZoneName.Main);*/
	}

	private void PressedNot()
	{
		/*DataBaseManager.instance.UpdateClient(GlobalObjects.UserMessage.clientes.login, GlobalObjects.UserMessage.clientes.pass, "false", XMLManager.Nit);
		UIController.Instance.MoveToZone(ZoneName.Main);*/
	}
}