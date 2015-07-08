using UnityEngine;
using System.Collections;

/// <summary>
/// Provide high-level control of the weapons;
/// Interfacing the low-level weapon implementations:
/// WeaponEditor - Weapon attributes container;
/// WeaponInventory - Weapon runtime state machine.
/// </summary>
[RequireComponent(typeof(WeaponEditor), typeof(WeaponInventory))]
public class WeaponBehaviorController : MonoBehaviour {

	public const string takeDamageNotification = "TakeDamage";

	public static LayerMask hitableLayers;


	/// <summary>
	/// Globle Fire() method.
	/// Return false if having no ammo left.
	/// </summary>
	public static bool Fire (int weaponIndex, int orientationIndex, Transform playerTransform) {
		if (!WeaponInventory.SubAmmo(weaponIndex)) {
			return false;
		}

		Weapon weapon = WeaponEditor.weapons[weaponIndex];

		Vector3 firePosition = weapon.GetFirePosition(orientationIndex) + playerTransform.position;
		Vector3 fireDirection = Orientation.GetDirection(orientationIndex);
		Quaternion fireRotation = Orientation.GetRotation(orientationIndex);

		// Instantiate fireEffect
		Instantiate(weapon.fireEffect, firePosition, fireRotation);

		// Apply recoil force
		playerTransform.GetComponent<Rigidbody>().AddForce(-fireDirection * weapon.recoilForce);

		for (int i=0; i<weapon.tragectoryCount; i++) {
			// Get random tragectory
			Vector3 randomFireDirection = weapon.firePercision.GetRandomFireDirection(fireDirection);

			if (weapon.bullet) {
				// If bullet exist, launch it
				GameObject bullet = (GameObject)Instantiate(weapon.bullet, firePosition, fireRotation);
				bullet.GetComponent<Rigidbody>().AddForce(randomFireDirection * weapon.hitForce);
			} else {
				// If not, raycast + apply force + instantiat hitEffect + apply damage
				RaycastHit hitInfo;
				if (Physics.Raycast(firePosition, randomFireDirection, out hitInfo, hitableLayers)) {
					hitInfo.rigidbody.AddForce(randomFireDirection * weapon.hitForce);
					
					Instantiate(weapon.hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
					
					hitInfo.collider.SendMessage(takeDamageNotification, weapon.hitDamage, SendMessageOptions.RequireReceiver);
				}
			}
		}

		return true;
	}

	/// <summary>
	/// Hide the Reload method in the WeaponInventory.
	/// </summary>
	public static bool Reload (int weaponIndex) {
		// todo: add sound support;

		return WeaponInventory.Reload(weaponIndex);
	}
}
