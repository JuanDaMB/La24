using System;
using IndieLevelStudio.Util;
using UnityEngine;
using UnityEngine.UI;

namespace IndieLevelStudio.IES.SlotMinigames.CatchLumine
{
	public enum StateLumine
	{
		WaitToCatched = 0,
		WaitToEvade = 1,
		Hide = 2,
	}

	public class Lumine : MonoBehaviour
	{
		public event Action OnGotLumine;

		public event Action OnHide;

		[SerializeField]
		[Range(0, 5)]
		private float timeToBeCatched = 0.5f;

		[SerializeField]
		[Range(0, 5)]
		private float timeToEvade = 0.5f;

		[SerializeField]
		[Range(0, 5)]
		private float hideTime = 0.5f;

		[SerializeField]
		[Range(0, 1)]
		private float transitionTime = 0.5f;

		[SerializeField]
		[Range(0, 1)]
		private float evadeMovement = 0.25f;

		[SerializeField]
		private LeanTweenType evadeEase;

		private RectTransform rectTransform;

		private Vector3 initialScale;

		public float HideTime
		{
			get { return hideTime; }
		}

		private Button mButton;

		private StateLumine mState;

		private void Start()
		{
			mButton = GetComponent<Button>();
			rectTransform = GetComponent<RectTransform>();

			mButton.onClick.AddListener(TryCatchLumine);

			initialScale = transform.localScale;
			LeanTween.alpha(rectTransform, 0, 0);
		}

		public void ShowLumine(Vector3 position)
		{
			transform.position = position;
			transform.localScale = initialScale;

			LeanTween.alpha(rectTransform, 1, transitionTime).setOnComplete(WaitToBeCatched);
		}

		private void WaitToBeCatched()
		{
			mState = StateLumine.WaitToCatched;
			mButton.enabled = true;

			TryInvoke(NameOf(WaitToEvade), timeToBeCatched);
		}

		private void WaitToEvade()
		{
			mState = StateLumine.WaitToEvade;

			TryInvoke(NameOf(Hide), timeToEvade);
		}

		private void GotLumine()
		{
			mState = StateLumine.Hide;
			mButton.enabled = false;

			CancelHide();

			LeanTween.alpha(rectTransform, 0, transitionTime);
			LeanTween.scale(rectTransform, initialScale * 1.5f, transitionTime);

			if (OnGotLumine != null)
				OnGotLumine();
		}

		private void Evade()
		{
			Vector3 evasion = transform.position;
			evasion.x += UnityEngine.Random.Range(-3, 3);
			evasion.y += UnityEngine.Random.Range(-3, 3);

			mButton.enabled = false;

			LeanTween.move(rectTransform, evasion, evadeMovement).setOnComplete(Hide).setEase(evadeEase);
			LeanTween.alpha(rectTransform, 0, evadeMovement);
		}

		private void Hide()
		{
			mState = StateLumine.Hide;
			mButton.enabled = false;

			CancelHide();
			LeanTween.alpha(rectTransform, 0, transitionTime).setOnComplete(() =>
			{
				if (OnHide != null)
					OnHide();
			});
		}

		public void Dissapear()
		{
			mState = StateLumine.Hide;
			mButton.enabled = false;

			CancelHide();
			LeanTween.alpha(rectTransform, 0, transitionTime).setOnComplete(CancelInvoke);
		}

		private void CancelHide()
		{
			if (IsInvoking(NameOf(Hide))) CancelInvoke(NameOf(Hide));
		}

		private void TryCatchLumine()
		{
			if (mState == StateLumine.WaitToCatched)
				GotLumine();
			else if (mState == StateLumine.WaitToEvade)
				Evade();
		}

		private void TryInvoke(string method, float time)
		{
			if (IsInvoking(method)) CancelInvoke(method);
			Invoke(method, time);
		}

		private string NameOf(Action method)
		{
			return Tools.NameOf(method);
		}
	}
}