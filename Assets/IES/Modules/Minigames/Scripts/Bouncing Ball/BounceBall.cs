using System;
using System.Collections.Generic;
using IndieLevelStudio.Common;
using IndieLevelStudio.Util;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace IndieLevelStudio.IES.SlotMinigames.BounceBall
{
	public class BounceBall : MinigameManager
	{
		public override event Action OnStart;

		public override event Action<bool, bool> OnFinish;

		[SerializeField]
		private Platform platform;

		[SerializeField]
		private EdgeAbyss floor;

		[SerializeField]
		private Camera gameCamera;

		[SerializeField]
		private CountdownTimer timer;

		[SerializeField]
		private Rigidbody2D ball;

		[SerializeField]
		private Button btnGameSpace;

		private bool isPlatformChecked;

		[SerializeField]
		private List<Transform> ballStartPoints;

		[SerializeField]
		[Range(0, 1)]
		private float deltaScalar = 0.1f;

		[SerializeField]
		[Range(1, 10)]
		private float ballImpulse = 0.1f;

		[SerializeField]
		[Range(0, 5)]
		private float hidePlatformTime = 2f;

		[SerializeField]
		private float gameTime;

		private Vector3 platformPosition;

		private Vector3 fingerPosition;

		private Vector3 mouseLastPos;

		private Vector3 mouseDeltaPos;

		private bool isPlaying;

		public override bool IsPlaying
		{
			get { return isPlaying; }
		}

		private void Start()
		{
			platform.OnBounce += OnBallBounce;
			floor.OnFallToAbyss += OnFallToAbyss;

			if (OnStart != null)
				OnStart();
			isPlaying = true;

			PositionateBall();

			timer.SetTimer(gameTime, OnPass);
		}

		private void OnPass()
		{
			ball.gameObject.SetActive(false);
			if (OnFinish != null)
				OnFinish(true, true);
		}

		private void Update()
		{
			platformPosition = platform.transform.position;
			MovePlatform();

			platform.transform.position = platformPosition;
		}

		private void PositionateBall()
		{
			ball.position = ballStartPoints[Random.Range(0, ballStartPoints.Count)].position;
		}

		private void MovePlatform()
		{
			if (Input.GetMouseButtonDown(0))
			{
				mouseLastPos = Input.mousePosition;
				fingerPosition = gameCamera.ScreenToWorldPoint(mouseLastPos);

				platformPosition.x = fingerPosition.x;
				platformPosition.y = fingerPosition.y;

				platform.transform.eulerAngles = Vector3.zero;

				TryInvoke(NameOf(HidePlatform), hidePlatformTime);
			}
			else if (Input.GetMouseButton(0) && isPlatformChecked)
			{
				mouseDeltaPos = Input.mousePosition - mouseLastPos;
				platform.transform.Rotate(-Vector3.forward * (mouseDeltaPos.x * deltaScalar));
			}
		}

		private void OnBallBounce(Rigidbody2D bounce)
		{
			bounce.velocity += Vector2.up * ballImpulse;
			HidePlatform();
		}

		private void OnFallToAbyss(Rigidbody2D fallen)
		{
			isPlaying = false;
			if (OnFinish != null)
				OnFinish(false, true);
		}

		private void TryInvoke(string method, float time)
		{
			if (IsInvoking(method)) CancelInvoke(method);
			Invoke(method, time);
		}

		private void HidePlatform()
		{
			platform.Hide();
			isPlatformChecked = false;
		}

		private string NameOf(Action method)
		{
			return Tools.NameOf(method);
		}
	}
}