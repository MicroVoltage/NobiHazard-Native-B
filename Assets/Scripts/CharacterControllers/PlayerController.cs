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

	PlayerAnimationController playerAnimationController;
	CharacterMovementController characterMovementController;

	void Start () {
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
		//RuntimeWeaponController.instance.Fire(weaponIndex, orientationIndex);
	}

	public void Recoil (float force) {
		characterMovementController.Rush(-Orientation.GetDirection(orientationIndex.GetOrientationIndex()) * force);
	}


}
