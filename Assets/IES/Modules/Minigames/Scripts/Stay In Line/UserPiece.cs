using System;
using UnityEngine;

namespace IndieLevelStudio.IES.SlotMinigames.StayInLine
{
	[RequireComponent(typeof(Collider2D))]
	public class UserPiece : MonoBehaviour
	{
		public event Action OnExitOfLine;

		public event Action OnFinishLine;

		private Vector3 dragPos;

		private Vector3 pos;

		private bool outOfLine;

		private bool isDead;

		private float secondsInactive;

		private void OnMouseDrag()
		{
			if (isDead) return;

			pos = transform.position;

			dragPos = StayInLine.Instance.GameCamera.ScreenToWorldPoint(Input.mousePosition);
			pos.x = dragPos.x;
			pos.y = dragPos.y;

			transform.position = pos;
		}

		private void Update()
		{
			if (!isDead)
			{
				if (outOfLine)
				{
					secondsInactive += Time.unscaledDeltaTime;
					if (secondsInactive >= StayInLine.Instance.OutOfLineTime)
					{
						isDead = true;
						if (OnExitOfLine != null)
							OnExitOfLine();
					}
				}
			}
		}

		private void OnTriggerStay2D(Collider2D col)
		{
			if (isDead) return;
			if (col.transform.name == "Finish Line" && !isDead)
			{
				isDead = true;
				if (OnFinishLine != null)
					OnFinishLine();
				return;
			}
			outOfLine = false;
			secondsInactive = 0;
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (isDead) return;
			outOfLine = true;
		}
	}
}