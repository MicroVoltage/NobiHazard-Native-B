using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterAnimationController))]
public class PlayerAnimationController : MonoBehaviour {
	// 0 - empty
	public string[] weaponNames;

	public string[] deadStateNames;

	int orientationIndex;
	int weaponIndex;
	int weaponStateIndex;
	
	CharacterAnimationController characterAnimationController;

	void Start () {
		characterAnimationController = GetComponent<CharacterAnimationController>();
	}

	public void SetOrientationIndex (int newOrientationIndex) {
		characterAnimationController.SetOrientationIndex(newOrientationIndex);
	}

	public void SetWeapon (int newWeaponIndex, int newWeaponStateIndex) {
		weaponIndex = newWeaponIndex;
		weaponStateIndex = newWeaponStateIndex;
		
		RefreshAnimationState();
	}

	public void SetWeaponIndex (int newWeaponIndex) {
		weaponIndex = newWeaponIndex;

		RefreshAnimationState();
	}

	public void SetWeaponStateIndex (int newWeaponStateIndex) {
		weaponStateIndex = newWeaponStateIndex;

		RefreshAnimationState();
	}

	public void Die (int deadStateIndex) {
		characterAnimationController.SetState(
			"dead" + CharacterAnimationController.s + deadStateNames[deadStateIndex]);
	}

	void RefreshAnimationState () {
		characterAnimationController.SetState(
			weaponNames[weaponIndex] + CharacterAnimationController.s + WeaponState.strings[weaponStateIndex]);
	}
}
