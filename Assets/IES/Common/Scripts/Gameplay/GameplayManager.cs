using UnityEngine;

namespace IndieLevelStudio.Common
{
	public abstract class GameplayManager : MonoBehaviour
	{
		public static GameplayManager Instance;

		protected virtual void Awake()
		{
			Instance = this;
			XMLModulesManager.LoadXML();
		}

		protected virtual void OnDestroy()
		{
			Instance = null;
		}
	}
}