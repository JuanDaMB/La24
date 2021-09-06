using System;
using IndieLevelStudio.Common;
using IndieLevelStudio.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IndieLevelStudio.IES.SlotMinigames.CatchLumine
{
	public class CatchTheLumine : MinigameManager
	{
		public override event Action<bool, bool> OnFinish;

		public override event Action OnStart;

		[SerializeField]
		private Lumine lumine;

		[SerializeField]
		private Camera gameCamera;

		[SerializeField]
		private CountdownTimer timer;

		[SerializeField]
		private float gameTime;

		private Vector3 luminePoint;

		private bool isPlaying;

		public override bool IsPlaying
		{
			get { return isPlaying; }
		}

		private void Start()
		{
			lumine.OnHide += OnHideLumine;

			lumine.OnGotLumine += OnGotLumine;
			timer.SetTimer(gameTime, OnLoseGame);

			TryInvoke(NameOf(AppearLumine), lumine.HideTime);

			if (OnStart != null)
				OnStart();
		}

		private void AppearLumine()
		{
			float x = Random.Range(20, Screen.width - 20);
			float y = Random.Range(20, Screen.height - 20);

			luminePoint = gameCamera.ScreenToWorldPoint(new Vector3(x, y, 5));
			lumine.ShowLumine(luminePoint);
		}

		private void OnGotLumine()
		{
			timer.Unable();
			TriggerFinish();

			if (OnFinish != null)
				OnFinish(true, true);
		}

		private void OnLoseGame()
		{
			timer.Unable();
			TriggerFinish();

			if (OnFinish != null)
				OnFinish(false, true);
		}

		private void TriggerFinish()
		{
			lumine.Dissapear();
			isPlaying = false;

			if (IsInvoking(NameOf(AppearLumine))) CancelInvoke(NameOf(AppearLumine));
		}

		private void OnHideLumine()
		{
			TryInvoke(NameOf(AppearLumine), lumine.HideTime);
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