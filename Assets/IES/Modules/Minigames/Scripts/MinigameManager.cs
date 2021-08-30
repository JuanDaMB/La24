using System;
using UnityEngine;

namespace IndieLevelStudio.IES.SlotMinigames
{
	public abstract class MinigameManager : MonoBehaviour
	{
		public abstract event Action OnStart;

		public abstract event Action<bool, bool> OnFinish;

		public abstract bool IsPlaying { get; }

		public virtual void Show()
		{
			gameObject.SetActive(true);
		}

		public virtual void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}