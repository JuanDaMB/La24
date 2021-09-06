using UnityEngine;

public class Border : MonoBehaviour
{
	public Animation anim;
	public bool isActiveBorder;

	private void Start()
	{
		isActiveBorder = false;
	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.tag == "Ball")
		{
			if (!isActiveBorder)
			{
				isActiveBorder = true;
				anim.Play("Press");
			}
		}
	}
}