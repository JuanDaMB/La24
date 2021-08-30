using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
	[SerializeField]
	private ParticleSystem particles;

	[SerializeField]
	private KeyCode interactKey;

	private void Update ()
	{
		if (Input.GetKeyDown (interactKey))
			InteractParticles ();
	}

	private void InteractParticles ()
	{
		if (particles == null) {
			Debug.LogWarning ("No particle system assigned to " + name);
			return;
		}

		if (particles.isPlaying)
			particles.Stop ();
		else
			particles.Play ();
	}
}
