using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Explosion2Manager : MonoBehaviour
{
	public static event Action OnExplosionEnd;

	private Vector3 explosionPos;

	public bool isActiveBase;

	private bool isActiveExplosion;

	private float powerExplosion;

	public int loopCount;

	public float time;

	public float rotateTo;

	public LeanTweenType ease;

	public void Explosion(float power)
	{
		isActiveExplosion = true;
		powerExplosion = power;
	}

	public void Spin()
	{
		LeanTween.rotateAround(gameObject, Vector3.up, rotateTo, time).setLoopCount(loopCount).setEase(ease);
	}

	public void ExplosionsEnds()
	{
		if (OnExplosionEnd != null)
			OnExplosionEnd();
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.collider.tag == "Dice")
		{
			if (isActiveExplosion)
			{
				isActiveExplosion = false;

				ContactPoint contact = collision.contacts[0];
				Vector3 pos = contact.point;
				Rigidbody diceRigibody = collision.collider.GetComponent<Rigidbody>();

				if (diceRigibody != null)
					diceRigibody.AddExplosionForce(powerExplosion, pos, 2.0f, 3.0f);
			}
			else
			{
				if (isActiveBase)
				{
					ContactPoint contact = collision.contacts[0];
					Vector3 pos = contact.point;
					Rigidbody diceRigibody = collision.collider.GetComponent<Rigidbody>();
					if (diceRigibody != null)
					{
						float explosionPower = Random.Range(300, 800);
						diceRigibody.transform.Find("ParticleHit").GetComponent<ParticleSystem>().Play();
						diceRigibody.AddExplosionForce(explosionPower, pos, 2.0f, 3.0f);
					}
				}
			}
		}
	}
}