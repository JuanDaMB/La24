using System;
using System.Collections.Generic;
using IndieLevelStudio.Common;
using IndieLevelStudio.Util;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace IndieLevelStudio.IES.SlotMinigames.GetAllCoins
{
	public enum PieceType
	{
		Neutral,
		WinCoin,
		LoseGame,
	}

	public class GetAllCoins : MinigameManager
	{
		public override event Action<bool, bool> OnFinish;

		public override event Action OnStart;

		[SerializeField]
		private List<CannonBall> prefabBalls;

		[SerializeField]
		private List<Transform> leftCannons;

		[SerializeField]
		private List<Transform> rightCannons;

		[SerializeField]
		private RectTransform coinsParent;

		[SerializeField]
		private CountdownTimer timer;

		private Vector3 cannonPos;

		private CannonBall currentBall;

		private bool isPlaying;

		private int coinsCounter;

		private int CoinsCounter
		{
			set
			{
				coinsCounter = value;
				coinsMeditor.text = coinsCounter + "/" + coinsObjective;
			}
			get { return coinsCounter; }
		}

		[SerializeField]
		private int coinsObjective;

		[SerializeField]
		private float gameTime;

		[SerializeField]
		private Text coinsMeditor;

		public override bool IsPlaying
		{
			get { return isPlaying; }
		}

		private void Start()
		{
			isPlaying = true;
			if (OnStart != null)
				OnStart();

			timer.SetTimer(gameTime, EndOfGame);

			CoinsCounter = 0;
			ShootTheBall();
		}

		private void ShootTheBall()
		{
			bool isLeft = Tools.TakeDesicion(50);

			int cannonIndex = Random.Range(0, isLeft ? leftCannons.Count : rightCannons.Count);
			int ballIndex = Random.Range(0, prefabBalls.Count);
			float randomTime = Random.Range(0.25f, 0.5f);
			float multiplier = Random.Range(5, 10);

			Vector3 sideDirection = isLeft ? Vector3.right : Vector3.left;

			cannonPos = isLeft ? leftCannons[cannonIndex].position : rightCannons[cannonIndex].position;
			currentBall = Instantiate(prefabBalls[ballIndex], coinsParent, false);
			currentBall.transform.position = cannonPos;
			currentBall.mRigidbody.AddForce((Vector3.up + sideDirection) * multiplier, ForceMode2D.Impulse);
			currentBall.OnGotPiece += OnGotPiece;

			if (isPlaying)
				Invoke(Tools.NameOf(ShootTheBall), randomTime);
		}

		private void OnGotPiece(PieceType pieceType)
		{
			switch (pieceType)
			{
				case PieceType.WinCoin:
					CoinsCounter++;
					if (coinsCounter >= coinsObjective)
						EndOfGame();
					break;

				case PieceType.LoseGame:
					EndOfGame();
					break;
			}
		}

		private void EndOfGame()
		{
			timer.Unable();

			isPlaying = false;

			if (OnFinish != null)
				OnFinish(CoinsCounter >= coinsObjective, true);
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			foreach (Transform son in leftCannons)
				Gizmos.DrawSphere(son.position, 0.15f);

			Gizmos.color = Color.blue;
			foreach (Transform son in rightCannons)
				Gizmos.DrawSphere(son.position, 0.15f);
		}
	}
}