using UnityEngine;

public class TBHoverChangeScale : MonoBehaviour
{
	public float hoverScaleFactor = 1.5f;
	private Vector3 originalScale = Vector3.one;

	private void Start()
	{
		// remember our original scale
		originalScale = transform.localScale;
	}

	private void OnFingerHover(FingerHoverEvent e)
	{
		if (e.Phase == FingerHoverPhase.Enter)
		{
			// apply scale modifier
			transform.localScale = hoverScaleFactor * originalScale;
		}
		else
		{
			// restore original scale
			transform.localScale = originalScale;
		}
	}
}