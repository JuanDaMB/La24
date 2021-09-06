using IndieLevelStudio.Common;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{

	[SerializeField]
	private Button btnLogin;

	[SerializeField]
	private Button btnRegister;

	[SerializeField]
	private InputField username;

	[SerializeField]
	private InputField password;

	private bool isActive = false;

	private void Start()
	{
		btnLogin.onClick.AddListener(TryLogin);
	}

	private void TryLogin()
	{
		if (!isActive)
		{
			isActive = true;
			/*if (XMLManager.ModeAdmin)
			{
				username.text = XMLManager.AdminUser;
				password.text = XMLManager.AdminPassword;
				if (username.text == XMLManager.AdminUser && password.text == XMLManager.AdminPassword)
				{
					Debug.Log("MODO ADMIN");
					GlobalObjects.IsModeAdmin = true;

					UIController.Instance.MoveToZone(ZoneName.Main);
				}
			}
			else
			{
				DataBaseManager.instance.CheckInternet();
				DataBaseManager.OnCompleteCheckInternet += InternetSuccess;
				DataBaseManager.OnErrorMessage += InternetFail;
			}*/
		}
	}

	private void InternetSuccess()
	{
		/*DataBaseManager.OnCompleteCheckInternet -= InternetSuccess;
		DataBaseManager.OnErrorMessage -= InternetFail;

		GlobalObjects.IsModeAdmin = false;
		DataBaseManager.instance.Login(username.text, password.text, XMLManager.Nit);

		DataBaseManager.OnCompleteLogin += LoginSuccess;
		DataBaseManager.OnErrorMessage += LoginFail;*/
	}

	private void InternetFail(string message)
	{
		isActive = false;
		DataBaseManager.OnCompleteCheckInternet -= InternetSuccess;
		DataBaseManager.OnErrorMessage -= InternetFail;

		Alert.Instance.Show("Mensaje:", message);
	}

	private void LoginFail(string message)
	{
		isActive = false;
		DataBaseManager.OnCompleteLogin -= LoginSuccess;
		DataBaseManager.OnErrorMessage -= LoginFail;

		Alert.Instance.Show("Mensaje:", message);
	}

	private void LoginSuccess()
	{
		DataBaseManager.OnCompleteLogin -= LoginSuccess;
		DataBaseManager.OnErrorMessage -= LoginFail;

		ResetInputsLogin();
		isActive = false;

		GoToNextScreen();
	}

	private void GoToNextScreen()
	{
		UIController.Instance.MoveToZone(GlobalObjects.UserMessage.clientes.habeasData ? ZoneName.Main : ZoneName.Rules);
	}

	private void ResetInputsLogin()
	{
		username.text = string.Empty;
		password.text = string.Empty;
	}
}