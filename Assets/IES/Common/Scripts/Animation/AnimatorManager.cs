using UnityEngine;

public abstract class AnimatorManager : MonoBehaviour
{
	protected Animator animator;

	protected virtual void Awake()
	{
		animator = GetComponent<Animator>();
		DesactiveAnimations();
	}

	public void ChangeAnimation<T>(T animation, bool isTrigger = false)
	{
		DesactiveAnimations();
		if (!isTrigger)
		{
			animator.SetBool(animation.ToString(), true);
		}
		else
		{
			animator.SetTrigger(animation.ToString());
		}
	}

	public void SetFloat<T>(T animation, float value, bool resetAnimations = false)
	{
		if (resetAnimations)
			DesactiveAnimations();

		animator.SetFloat(animation.ToString(), value);
	}

	public void SetBool<T>(T animation, bool value, bool resetAnimations = false)
	{
		if (resetAnimations)
			DesactiveAnimations();

		animator.SetBool(animation.ToString(), value);
	}

	protected abstract void DesactiveAnimations();

	protected virtual void OnCompleteAnimation(string animationName)
	{
	}
}