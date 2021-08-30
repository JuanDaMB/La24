using System;
using UnityEngine;

namespace IndieLevelStudio.IES.SlotMinigames.BounceBall
{
	[RequireComponent(typeof(Collider2D))]
	public class EdgeAbyss : MonoBehaviour
	{
		public event Action<Rigidbody2D> OnFallToAbyss;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (OnFallToAbyss != null)
				OnFallToAbyss(collision.transform.GetComponent<Rigidbody2D>());
		}
	}
}