using UnityEngine;
using System.Collections;

/// <summary>
/// Capsulate all the implimentation of a player.
/// </summary>
[RequireComponent(typeof(PlayerAnimationController), typeof(CharacterMovementController))]
public class PlayerController : MonoBehaviour {
	public float maxMoveForce;
	public float jumpForce;
	
	OrientationIndex orientationIndex = new OrientationIndex();

	PlayerWeaponController playerWeaponController;
	PlayerAnimationController playerAnimationController;
	CharacterMovementController characterMovementController;

	void Start () {
		playerWeaponController = GetComponent<PlayerWeaponController>();
		playerAnimationController = GetComponent<PlayerAnimationController>();
		characterMovementController = GetComponent<CharacterMovementController>();
	}
	
	/// <summary>
	/// normalizedForce.magnitude <= 1.
	/// </summary>
	public void Move (Vector2 normalizedForce) {
		playerAnimationController.SetOrientationIndex(orientationIndex.RefreshOrientationIndex(normalizedForce));
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

	public void Fire () {
		playerAnimationController.SetWeaponStateIndex(WeaponState.fire);
		playerWeaponController.Fire(orientationIndex.GetOrientationIndex());
	}

	public void Reload () {
		playerAnimationController.SetWeaponStateIndex(WeaponState.reload);
		playerWeaponController.Reload();
	}

	public void NextWeapon (int weaponIndex) {
		playerAnimationController.SetWeapon(weaponIndex, WeaponState.draw);
	}

	public void TakeDamage (float damage) {

	}
}
