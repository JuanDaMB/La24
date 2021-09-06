using UnityEngine;

public abstract class UIZone : MonoBehaviour
{
	public abstract ZoneName Zone { get; }

	public virtual void SetEnabled(bool enabled)
	{
		gameObject.SetActive(enabled);
	}
}