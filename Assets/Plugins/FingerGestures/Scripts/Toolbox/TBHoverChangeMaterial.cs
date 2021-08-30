using UnityEngine;

public class TBHoverChangeMaterial : MonoBehaviour
{
	public Material hoverMaterial;
	private Material normalMaterial;

	private void Start()
	{
		// remember our original material
		normalMaterial = GetComponent<Renderer>().sharedMaterial;
	}

	private void OnFingerHover(FingerHoverEvent e)
	{
		if (e.Phase == FingerHoverPhase.Enter)
			GetComponent<Renderer>().sharedMaterial = hoverMaterial; // show hover-state material
		else
			GetComponent<Renderer>().sharedMaterial = normalMaterial; // restore original material
	}
}