using UnityEngine;
using System.Collections;

/// <summary>
/// characterName + state + orientationIndex
/// Interfacing GenericAnimationController.
/// </summary>
[RequireComponent(typeof(GenericAnimationController))]
public class CharacterAnimationController : MonoBehaviour {
	public const string s = "-";

	public string characterName;

	int orientationIndex;
	string state;

	GenericAnimationController genericAnimationController;

	void Start () {
		genericAnimationController = GetComponent<GenericAnimationController>();
	}

	public void StackOrientationIndex (int newOrientationIndex) {
		orientationIndex = newOrientationIndex;
	}

	public void StackState (string newState) {
		state = newState;
	}

	public void ForceApplyAnimation () {
		genericAnimationController.ForcePlayAnimation(
			characterName + s + state + s + Orientation.strings[orientationIndex]);
	}

	public void SetOrientationIndex (int newOrientationIndex) {
		StackOrientationIndex(newOrientationIndex);

		ApplyAnimation();
	}

	public void SetState (string newState) {
		StackState(newState);

		ApplyAnimation();
	}

	void ApplyAnimation () {
		genericAnimationController.PlayAnimation(
			characterName + s + state + s + Orientation.strings[orientationIndex]);
	}
}
