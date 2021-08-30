using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneReloader : MonoBehaviour 
{
	private void Start() 
	{
		Screen.fullScreen = true;	
	}

	private void Update () 
	{
		if(Input.GetKeyDown(KeyCode.R))
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
