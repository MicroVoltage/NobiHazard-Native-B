using UnityEngine;
using System.Collections;

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
			Debug.Log(animationName);
			animator.Play(animationName, 0);
		}
	}
}
