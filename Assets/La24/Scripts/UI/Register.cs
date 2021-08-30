using IndieLevelStudio.Common;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{

	[SerializeField]
	private Button btnBack;

	[SerializeField]
	private Button btnRegister;

	[SerializeField]
	private Toggle tglTerms;

	public InputField inputnamecomplete;
	public InputField inputidenty;
	public InputField inputemail;
	public InputField inputphone;
	public InputField inputusername;
	public InputField inputpassword;
	public InputField inputpassword2;
	public InputField inputbirthdate;

	private void Start()
	{
		btnRegister.onClick.AddListener(TryRegister);
	}

	private void TryRegister()
	{
		if (inputnamecomplete.text == "" || inputidenty.text == "" || inputphone.text == "" || inputbirthdate.text == "")
		{
			Alert.Instance.Show("Mensaje", "Todos los campos son obligatorios");
		}
		else
		{
			if (!tglTerms.isOn)
			{
				Alert.Instance.Show("Mensaje", "Debes aceptar los términos y condiciones");
				return;
			}
			/*DataBaseManager.instance.SaveClient(inputidenty.text, inputnamecomplete.text, inputusername.text, inputpassword.text, inputemail.text, inputphone.text, inputbirthdate.text, "false", XMLManager.Nit);
			DataBaseManager.OnCompleteRegister += SuccessRegister;
			DataBaseManager.OnErrorMessage += FailRegister;*/
		}
	}

	private void OnEnable()
	{
		ResetInputsRegister();
	}

	public void ResetInputsRegister()
	{
		inputnamecomplete.text = "";
		if (inputidenty)
		{
			inputidenty.text = "";
		}

		inputemail.text = "";
		if (inputphone)
		{
			inputphone.text = "";
		}
		inputusername.text = "";
		inputpassword.text = "";
		inputpassword2.text = "";
		if (inputbirthdate)
		{
			inputbirthdate.text = "";
		}
	}

	private void SuccessRegister()
	{
		DataBaseManager.OnCompleteRegister -= SuccessRegister;
		DataBaseManager.OnErrorMessage -= FailRegister;

		UIController.Instance.MoveToZone(GlobalObjects.UserMessage.clientes.habeasData ? ZoneName.Main : ZoneName.Rules);

		Alert.Instance.Show("Mensaje", "Cuenta creada, entrando con la nueva cuenta");
	}

	private void FailRegister(string message)
	{
		DataBaseManager.OnCompleteRegister -= SuccessRegister;
		DataBaseManager.OnErrorMessage -= FailRegister;

		Alert.Instance.Show("Mensaje", message);
	}

	private bool CheckerDefaultsInputs(string text)
	{
		bool value = false;
		if (text == "INGRESA TU NOMBRE DE USUARIO")
		{
			value = true;
		}
		else if (text == "INGRESA TU CONTRASEÑA")
		{
			value = true;
		}
		else if (text == "Ingresa tu nombre completo")
		{
			value = true;
		}
		else if (text == "Ingresa nombre de usuario")
		{
			value = true;
		}
		else if (text == "Ingresa tu email")
		{
			value = true;
		}
		else if (text == "Ingresa numero celular")
		{
			value = true;
		}
		else if (text == "Ingresa tu cedula")
		{
			value = true;
		}
		else if (text == "Dia/Mes/Año")
		{
			value = true;
		}
		else if (text == "Ingresa contraseña")
		{
			value = true;
		}
		else if (text == "Confirma contraseña")
		{
			value = true;
		}
		return value;
	}
}