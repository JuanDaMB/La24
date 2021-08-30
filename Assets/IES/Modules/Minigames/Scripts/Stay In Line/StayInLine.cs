using System;
using UnityEngine;
using UnityEngine.UI;

namespace IndieLevelStudio.IES.SlotMinigames.StayInLine
{
	public class StayInLine : MinigameManager
	{
		public static StayInLine Instance;

		public override event Action<bool, bool> OnFinish;

		public override event Action OnStart;

		[SerializeField]
		private Transform lineMarquee;

		[SerializeField]
		private UserPiece piece;

		[SerializeField]
		private Camera gameCamera;

		[SerializeField]
		private Text resultText;

		[SerializeField]
		[Range(0, 10)]
		private float marqueeSpeed;

		[SerializeField]
		[Range(0, 5)]
		private float marqueeDelay = 2;

		[SerializeField]
		[Range(0, 1)]
		private float outOfLineTime = 0.15f;

		private Vector3 finishLinePos;

		private bool isPlaying;

		public Camera GameCamera
		{
			get { return gameCamera; }
		}

		public float OutOfLineTime
		{
			get { return outOfLineTime; }
		}

		public override bool IsPlaying
		{
			get { return isPlaying; }
		}

		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			piece.OnExitOfLine += OnExitOfLine;
			piece.OnFinishLine += OnFinishLine;

			MoveMarquee();

			isPlaying = true;
			if (OnStart != null)
				OnStart();
		}

		private void MoveMarquee()
		{
			finishLinePos = lineMarquee.GetChild(lineMarquee.childCount - 1).position;
			LeanTween.moveX(lineMarquee.gameObject, lineMarquee.position.x - finishLinePos.x, 0.5f).setSpeed(marqueeSpeed).setDelay(marqueeDelay);
		}

		private void OnFinishLine()
		{
			LeanTween.cancel(lineMarquee.gameObject);

			resultText.text = "¡GANADOR!";

			isPlaying = false;
			if (OnFinish != null)
				OnFinish(true, true);
		}

		private void OnExitOfLine()
		{
			LeanTween.cancel(lineMarquee.gameObject);

			resultText.text = "HAS PERDIDO";

			isPlaying = false;
			if (OnFinish != null)
				OnFinish(false, true);
		}
	}
}