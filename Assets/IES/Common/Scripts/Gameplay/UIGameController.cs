using System.Collections.Generic;
using UnityEngine;

namespace IndieLevelStudio.Common
{
	public abstract class UIGameController : MonoBehaviour
	{
		public static UIGameController Instance;

		[SerializeField]
		private List<UIZone> uiZones;

		protected virtual void Awake()
		{
			Instance = this;
		}

		public void MoveToZone(ZoneName newZone)
		{
			uiZones.ForEach(z => { z.SetEnabled(z.Zone == newZone); });
		}

		public void DisableAll()
		{
			uiZones.ForEach(z => z.SetEnabled(false));
		}

		protected virtual void OnDestroy()
		{
			Instance = null;
		}
	}
}