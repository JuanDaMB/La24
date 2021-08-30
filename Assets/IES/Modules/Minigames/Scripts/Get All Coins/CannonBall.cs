using System;
using UnityEngine;
using UnityEngine.UI;

namespace IndieLevelStudio.IES.SlotMinigames.GetAllCoins
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class CannonBall : MonoBehaviour
	{
		public event Action<PieceType> OnGotPiece;

		[SerializeField]
		public Rigidbody2D mRigidbody;

		[SerializeField]
		private Button mButton;

		[SerializeField]
		private PieceType type;

		private Vector3 initialScale;

		private RectTransform rectTransform;

		private void Start()
		{
			rectTransform = GetComponent<RectTransform>();

			initialScale = transform.localScale;
			mButton.onClick.AddListener(CatchPiece);

			Destroy(gameObject, 10);
		}

		private void CatchPiece()
		{
			mButton.enabled = false;
			mRigidbody.simulated = false;

			LeanTween.scale(rectTransform, initialScale * 2.5f, 0.5f);
			LeanTween.alpha(rectTransform, 0, 0.5f).setOnComplete(() => Destroy(gameObject));

			if (OnGotPiece != null)
				OnGotPiece(type);
		}

		private void OnDestroy()
		{
			OnGotPiece = null;
		}
	}
}