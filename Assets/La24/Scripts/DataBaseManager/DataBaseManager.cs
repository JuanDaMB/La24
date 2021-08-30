using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using IndieLevelStudio.Common;

public class DataBaseManager : MonoBehaviour
{
	public static event Action OnCompleteLogin;

	public static event Action OnCompleteRegister;

	public static event Action OnCompleteUpdateCliente;

	public static event Action OnCompleteCheckInternet;

	public static event Action OnCompletePrint;

	public static event Action<string> OnErrorMessage;

	public static DataBaseManager instance;

	private WWWForm form;
	private string wsUrl = string.Empty;

	private void Awake ()
	{
		instance = this;
	}

	public void Login (string user, string pass, string enterprice)
	{
		print ("Call Login");
		object[] parms = new object[3] { user, pass, enterprice };
		StartCoroutine (CallLogin (parms));
	}

	private IEnumerator CallLogin (object[] parms)
	{
		form = new WWWForm ();
		form.AddField ("login", (string)parms [0]);
		form.AddField ("pass", (string)parms [1]);
		form.AddField ("codigo_empresa", (string)parms [2]);

		string url = wsUrl + "api/cliente/login";

		UnityWebRequest www = UnityWebRequest.Post (url, form);
		yield return www.SendWebRequest ();

		Debug.Log ("JSON: " + www.downloadHandler.text);
		Debug.Log ("CODE: " + www.responseCode);
		if (www.responseCode == 200) {
			GlobalObjects.UserMessage = JsonUtility.FromJson<Message> (www.downloadHandler.text);
			Debug.Log ("User Name: " + GlobalObjects.UserMessage.clientes.nombre);
			if (GlobalObjects.UserMessage.statusDTO.code == "00") {
				if (OnCompleteLogin != null) {
					OnCompleteLogin ();
				}
			} else {
				if (OnErrorMessage != null) {
					OnErrorMessage (GlobalObjects.UserMessage.statusDTO.message);
				}
			}
		} else {
			DataBaseErrorMessage errorMessage = JsonUtility.FromJson<DataBaseErrorMessage> (www.downloadHandler.text);
			if (OnErrorMessage != null) {
				OnErrorMessage (errorMessage.error_description);
			} else
				OnErrorMessage ("No se pudo obtener correctamente la respuesta del servidor.");
		}
	}

	public void PrintWS (string nombre, string cedula, string razonsocial, string consecutivo, string premio_Obtenido, string premio_Numero, string mensaje, string correo, string correo_sede)
	{
		print ("Call Print");
		object[] parms = new object[9] {
			nombre,
			cedula,
			razonsocial,
			consecutivo,
			premio_Obtenido,
			premio_Numero,
			mensaje,
			correo,
			correo_sede
		};
		StartCoroutine (CallPrint (parms));
	}

	private IEnumerator CallPrint (object[] parms)
	{
		form = new WWWForm ();
		form.AddField ("nombre", (string)parms [0]);
		form.AddField ("cedula", (string)parms [1]);
		form.AddField ("razon_social", (string)parms [2]);
		form.AddField ("consecutivo", (string)parms [3]);
		form.AddField ("premio_obtenido", (string)parms [4]);
		form.AddField ("premio_numero", (string)parms [5]);
		form.AddField ("mensaje", (string)parms [6]);
		form.AddField ("correo", (string)parms [7]);
		form.AddField ("correo_sede", (string)parms [8]);

		string url = wsUrl + "api/general/imprimir";
		Debug.Log ("URL: " + url);
		UnityWebRequest www = UnityWebRequest.Post (url, form);

		yield return www.SendWebRequest ();
		Debug.Log ("JSON: " + www.downloadHandler.text);
		Debug.Log ("CODE: " + www.responseCode);
		DataBaseGeneralMessage printObject = JsonUtility.FromJson<DataBaseGeneralMessage> (www.downloadHandler.text);
		if (www.responseCode == 200) {
			if (printObject.statusDTO.code == "00") {
				if (OnCompletePrint != null) {
					OnCompletePrint ();
				}
			} else {
				if (OnErrorMessage != null) {
					try {
						OnErrorMessage (GlobalObjects.UserMessage.statusDTO.message);
					} catch (Exception) {
						OnErrorMessage ("No se pudo enviar el recibo correctamente.");
					}
				}
			}
		} else {
			DataBaseErrorMessage errorMessage = JsonUtility.FromJson<DataBaseErrorMessage> (www.downloadHandler.text);
			if (OnErrorMessage != null) {
				if (errorMessage != null)
					OnErrorMessage (errorMessage.error_description);
				else
					OnErrorMessage ("No se pudo obtener correctamente la respuesta del servidor.");
			} else
				OnErrorMessage ("No se pudo obtener correctamente la respuesta del servidor.");
		}
	}

	public void SaveClient (string numero_identificacion, string nombre, string login, string pass, string email, string celular, string fecha_nacimiento, string habeas_data, string codigo_empresa)
	{
		object[] parms = new object[9] {
			numero_identificacion,
			nombre,
			login,
			pass,
			email,
			celular,
			fecha_nacimiento,
			habeas_data,
			codigo_empresa
		};
		StartCoroutine (CallSaveClient (parms));
	}

	private IEnumerator CallSaveClient (object[] parms)
	{
		form = new WWWForm ();
		form.AddField ("numero_identificacion", (string)parms [0]);
		form.AddField ("nombre", (string)parms [1]);
		form.AddField ("login", (string)parms [2]);
		form.AddField ("pass", (string)parms [3]);
		form.AddField ("email", (string)parms [4]);
		form.AddField ("celular", (string)parms [5]);
		form.AddField ("fecha_nacimiento", (string)parms [6]);
		form.AddField ("habeas_data", (string)parms [7]);
		form.AddField ("codigo_empresa", (string)parms [8]);

		print ("Call SaveClient");
		string url = wsUrl + "api/cliente/guardarCliente";
		UnityWebRequest www = UnityWebRequest.Post (url, form);
		yield return www.SendWebRequest ();
		Debug.Log ("JSON: " + www.downloadHandler.text);
		Debug.Log ("CODE: " + www.responseCode);

		if (www.responseCode == 200) {
			GlobalObjects.UserMessage = JsonUtility.FromJson<Message> (www.downloadHandler.text);
			Debug.Log ("User Name: " + GlobalObjects.UserMessage.clientes.nombre);
			if (GlobalObjects.UserMessage.statusDTO.code == "00") {
				if (OnCompleteRegister != null) {
					OnCompleteRegister ();
				}
			} else {
				if (OnErrorMessage != null) {
					OnErrorMessage (GlobalObjects.UserMessage.statusDTO.message);
				}
			}
		} else {
			DataBaseErrorMessage errorMessage = JsonUtility.FromJson<DataBaseErrorMessage> (www.downloadHandler.text);
			if (OnErrorMessage != null) {
				OnErrorMessage (errorMessage.error_description);
			} else
				OnErrorMessage ("No se pudo obtener correctamente la respuesta del servidor.");
		}
	}

	public void UpdateClient (string login, string pass, string habeas_data, string codigo_empresa)
	{
		object[] parms = new object[4] { login, pass, habeas_data, codigo_empresa };
		StartCoroutine (CallUpdateClient (parms));
	}

	private IEnumerator CallUpdateClient (object[] parms)
	{
		form = new WWWForm ();
		for (int i = 0; i < parms.Length; i++) {
			Debug.Log (parms [i]);
		}

		string habeasdata = parms [2].ToString ();
		form.AddField ("login", (string)parms [0]);
		form.AddField ("pass", (string)parms [1]);
		form.AddField ("habeas_data", habeasdata);
		form.AddField ("codigo_empresa", (string)parms [3]);

		print ("Call SaveClient");
		string url = wsUrl + "api/cliente/actualizarCliente2";
		UnityWebRequest www = UnityWebRequest.Post (url, form);
		yield return www.SendWebRequest ();
		Debug.Log ("JSON: " + www.downloadHandler.text);
		Debug.Log ("CODE: " + www.responseCode);
		if (www.responseCode == 200) {
			GlobalObjects.UserMessage = JsonUtility.FromJson<Message> (www.downloadHandler.text);
			Debug.Log ("User Name: " + GlobalObjects.UserMessage.clientes.nombre);
			if (GlobalObjects.UserMessage.statusDTO.code == "00") {
				if (OnCompleteUpdateCliente != null) {
					OnCompleteUpdateCliente ();
				}
			} else {
				if (OnErrorMessage != null) {
					OnErrorMessage (GlobalObjects.UserMessage.statusDTO.message);
				}
			}
		} else {
			DataBaseErrorMessage errorMessage = JsonUtility.FromJson<DataBaseErrorMessage> (www.downloadHandler.text);
			if (OnErrorMessage != null) {
				OnErrorMessage (errorMessage.error_description);
			} else
				OnErrorMessage ("No se pudo obtener correctamente la respuesta del servidor.");
		}
	}

	public void CheckInternet ()
	{
		print ("CheckInternet");
		StartCoroutine (CallCheckInternet ());
	}

	private IEnumerator CallCheckInternet ()
	{
		form = new WWWForm ();
		form.AddField ("login", "");
		string url = wsUrl + "api/general/validar";

		UnityWebRequest www = UnityWebRequest.Post (url, form);

		yield return www.SendWebRequest ();
		Debug.Log ("JSON: " + www.downloadHandler.text);
		Debug.Log ("CODE: " + www.responseCode);
		if (www.responseCode == 200) {
			DataBaseGeneralMessage internetObject = JsonUtility.FromJson<DataBaseGeneralMessage> (www.downloadHandler.text);
			Debug.Log ("Status DTO: " + internetObject.statusDTO.code);
			if (internetObject.statusDTO.code == "00") {
				if (OnCompleteCheckInternet != null) {
					OnCompleteCheckInternet ();
				}
			} else {
				if (OnErrorMessage != null) {
					OnErrorMessage (GlobalObjects.UserMessage.statusDTO.message);
				}
			}
		} else {
			DataBaseErrorMessage errorMessage = JsonUtility.FromJson<DataBaseErrorMessage> (www.downloadHandler.text);
			if (OnErrorMessage != null) {
				if (errorMessage != null)
					OnErrorMessage (errorMessage.error_description);
				else
					OnErrorMessage ("No se pudo obtener correctamente la respuesta del servidor.");
			}
		}
	}

	public static void StopAllProccess ()
	{
		instance.StopAllCoroutines ();
		instance.ClearEvents ();
	}

	private void ClearEvents ()
	{
		OnCompleteLogin = null;
		OnCompleteRegister = null;
		OnCompleteUpdateCliente = null;
		OnCompleteCheckInternet = null;
		OnErrorMessage = null;
		OnCompletePrint = null;
	}

	private void OnApplicationQuit ()
	{
		ClearEvents ();
	}

	private void OnDestroy ()
	{
		ClearEvents ();
	}
}