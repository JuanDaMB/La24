using IndieLevelStudio.BlackJack.Settings;
using UnityEditor;
using UnityEngine;

public class ILSMenuItems : Editor
{
	[MenuItem("ILS/Delete PlayerPrefs")]
	public static void DeletePlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
		Debug.Log("Player prefs cleaned");
	}

	[MenuItem("ILS/Create/Game Settings")]
	public static void CreateAsset()
	{
		ScriptableObjectUtility.CreateAsset<GameSettings>();
	}
}