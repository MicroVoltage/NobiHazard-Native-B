using UnityEngine;
using System.Collections;

/// <summary>
/// The base class interfacing the Animator.
/// </summary>
public class GenericAnimationController : MonoBehaviour {
	public Animator animator;

	string lastAnimationName;
	
	public void PlayAnimation (string animationName) {
		if (animationName == lastAnimationName) {
			return;
		}
		
		lastAnimationName = animationName;

		AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
		if (!animatorStateInfo.IsName(animationName)) {
			animator.Play(animationName, 0);
		}
	}

	public void ForcePlayAnimation (string animationName) {
		lastAnimationName = animationName;
		
		animator.Play(animationName, 0);
	}
}