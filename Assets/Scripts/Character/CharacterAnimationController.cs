using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericAnimationController))]
public class CharacterAnimationController : MonoBehaviour {
	public string characterName;

	int orientationIndex;
	public static string s = "-";

	string state;

	GenericAnimationController genericAnimationController;

	void Start () {
		genericAnimationController = GetComponent<GenericAnimationController>();
	}

	public void SetOrientationIndex (int newOrientationIndex) {
		orientationIndex = newOrientationIndex;

		ApplyAnimation();
	}

	public void SetState (string newState) {
		state = newState;

		ApplyAnimation();
	}

	void ApplyAnimation () {
		genericAnimationController.PlayAnimation(
			characterName + s + state + s + Orientation.strings[orientationIndex]);
	}
}
