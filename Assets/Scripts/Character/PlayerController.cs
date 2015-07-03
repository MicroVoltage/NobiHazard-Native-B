using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerAnimationController), typeof(CharacterMovementController))]
public class PlayerController : MonoBehaviour {
	public float maxMoveForce;
	public float jumpForce;
	
	int orientationIndex;

	float tolerance = 0.01f;

	PlayerAnimationController playerAnimationController;
	CharacterMovementController characterMovementController;

	void Start () {
		playerAnimationController = GetComponent<PlayerAnimationController>();
		characterMovementController = GetComponent<CharacterMovementController>();
	}

	/* normalizedForce = magnitude less than 1
	 * magnitude of 1 means full speed
	 */
	public void Move (Vector2 normalizedForce) {
		playerAnimationController.SetOrientationIndex(CalculateOrientationIndex(normalizedForce));
		playerAnimationController.SetWeaponStateIndex(WeaponState.walk);

		characterMovementController.Move(normalizedForce * maxMoveForce);
	}

	public void Stop () {
		playerAnimationController.SetWeaponStateIndex(WeaponState.idle);

		characterMovementController.Stop();
	}

	public void Jump () {
		characterMovementController.Jump(jumpForce);
	}

	public void ChangeWeapon (int weaponIndex) {
		playerAnimationController.SetWeapon(weaponIndex, WeaponState.draw);
	}

	public void ReloadWeapon (int weaponIndex) {
		playerAnimationController.SetWeapon(weaponIndex, WeaponState.draw);
	}

	public void Disarm () {
		playerAnimationController.SetWeapon(0, WeaponState.idle);
	}

	public void Fire (int weaponIndex) {
		playerAnimationController.SetWeaponStateIndex(WeaponState.fire);
		RuntimeWeaponController.instance.Fire(weaponIndex, orientationIndex);
	}

	public void Recoil (float force) {
		characterMovementController.Rush(-Orientation.GetDirection(orientationIndex) * force);
	}

	int CalculateOrientationIndex (Vector2 direction) {
		if (Mathf.Abs(Mathf.Abs(direction.x) - Mathf.Abs(direction.y)) < tolerance) {
			return orientationIndex;
		}

		// string[] orientationNames = {"back", "right", "left", "front"};
		bool positiveY = direction.y > 0;
		bool biggerX = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
		if (direction.x > 0) {
			if (positiveY) {
				// ^>
				if (biggerX) {
					orientationIndex = Orientation.right;
				} else {
					orientationIndex = Orientation.back;
				}
			} else {
				// v>
				if (biggerX) {
					orientationIndex = Orientation.right;
				} else {
					orientationIndex = Orientation.front;
				}
			}
		} else {
			if (positiveY) {
				//<^
				if (biggerX) {
					orientationIndex = Orientation.left;
				} else {
					orientationIndex = Orientation.back;
				}
			} else {
				//<v
				if (biggerX) {
					orientationIndex = Orientation.left;
				} else {
					orientationIndex = Orientation.front;
				}
			}
		}

		return orientationIndex;
	}
}
