using System.Collections.Generic;
using UnityEngine;

namespace IndieLevelStudio.Common
{
	public class ShineDotsAnimation : MonoBehaviour
	{
		[SerializeField]
		private List<Transform> dots;

		[SerializeField]
		private float scaleMaxObjects = 1.5f;

		private Dictionary<Transform, Vector3> scales;

		private Transform dot;

		private Transform newDot;

		private bool setuped;
		public float time;

		private void Awake()
		{
			scales = new Dictionary<Transform, Vector3>();
			dots.ForEach(dot =>
			{
				scales[dot] = dot.localScale;

				LeanTween.cancel(dot.gameObject);
				dot.localScale = Vector3.zero;
			});
			PlayDotAnimation();

			setuped = true;
		}

		private void OnEnable()
		{
			if (!setuped) return;

			dots.ForEach(dot =>
			{
				LeanTween.cancel(dot.gameObject);
				dot.localScale = Vector3.zero;
			});
			PlayDotAnimation();
		}

		public void PlayDotAnimation()
		{
			while (newDot == dot)
				newDot = dots[Random.Range(0, dots.Count)];

			dot = newDot;
			LeanTween.scale(dot.gameObject, scales[dot] * scaleMaxObjects, time * 0.5f).setOnComplete(() =>
			{
				LeanTween.scale(dot.gameObject, Vector3.zero, time * 0.5f).setOnComplete(PlayDotAnimation);
			});
		}
	}
}