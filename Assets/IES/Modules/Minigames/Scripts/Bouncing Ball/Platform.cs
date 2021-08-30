using System;
using UnityEngine;

namespace IndieLevelStudio.IES.SlotMinigames.BounceBall
{
	[RequireComponent(typeof(Collider2D))]
	public class Platform : MonoBehaviour
	{
		public event Action<Rigidbody2D> OnBounce;

		private Vector3 initialPos;

		private Collider2D myCollider;

		[SerializeField]
		[Range(0, 2)]
		private float hideTime = 0.5f;

		public bool IsEnabled
		{
			get; private set;
		}

		private void Start()
		{
			initialPos = transform.position;
			myCollider = GetComponent<Collider2D>();
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (OnBounce != null)
				OnBounce(collision.transform.GetComponent<Rigidbody2D>());
		}

		private void Rebirth()
		{
			myCollider.enabled = true;
			IsEnabled = true;

			LeanTween.alpha(gameObject, 1, 0f);
			transform.position = initialPos;
		}

		public void Hide()
		{
			myCollider.enabled = false;
			IsEnabled = false;

			LeanTween.alpha(gameObject, 0, hideTime).setOnComplete(Rebirth);
		}
	}
}