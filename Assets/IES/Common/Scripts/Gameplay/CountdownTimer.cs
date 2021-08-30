using System;
using UnityEngine;
using UnityEngine.UI;

namespace IndieLevelStudio.Common
{
	public class CountdownTimer : MonoBehaviour
	{
		private Action onTimeExpires;


		[SerializeField]
		private Image indicator;

		private DateTime nextTime;

		private TimeSpan difference;

		private float disabledSeconds;

		private bool isRunning;

		public bool HasStarted
		{
			get; private set;
		}

		public double TotalSeconds
		{
			get { return difference.TotalSeconds; }
		}

		private float miliseconds;

		public void SetTimer(float seconds, Action onTimeExpires)
		{
			HasStarted = true;
			this.onTimeExpires = onTimeExpires;
			miliseconds = seconds * 1000;
			nextTime = DateTime.Now.AddSeconds(seconds);

			isRunning = true;
			gameObject.SetActive(isRunning);
		}

		public void Pause()
		{
			LeanTween.pause(indicator.gameObject);
			disabledSeconds = Time.time;
			isRunning = false;
		}

		public void Resume()
		{
			LeanTween.resume(indicator.gameObject);
			nextTime = nextTime.AddSeconds(Time.time - disabledSeconds);
			isRunning = true;
		}

		private void Update()
		{
			if (!isRunning)
				return;

			if (DateTime.Compare(DateTime.Now, nextTime) > 0)
			{
				isRunning = false;

				onTimeExpires();
				Unable();
			}
			difference = nextTime.Subtract(DateTime.Now);

			indicator.fillAmount = (float)difference.TotalMilliseconds / miliseconds;
		}

		public void Unable()
		{
			HasStarted = false;
			isRunning = false;

			gameObject.SetActive(isRunning);
		}
	}
}