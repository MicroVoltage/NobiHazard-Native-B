using UnityEngine;
using System.Collections;

public class RuntimeWeaponController : MonoBehaviour {
	public static RuntimeWeaponController instance;
	
	public LayerMask hitableLayers;

	void Awake () {
		instance = this;
	}

	public void Fire (int weaponIndex, int orientationIndex) {
		Weapon weapon = WeaponEditor.instance.weapons[weaponIndex];

		Vector3 firePosition = weapon.GetFirePosition(orientationIndex);
		Vector3 fireDirection = Orientation.GetDirection(orientationIndex);
		Quaternion fireRotation = Orientation.GetRotation(orientationIndex);

		Instantiate(weapon.fireEffect, firePosition, fireRotation);

		RaycastHit hitInfo;
		if (Physics.Raycast(firePosition, fireDirection, out hitInfo, hitableLayers)) {
			hitInfo.rigidbody.AddForce(fireDirection * weapon.hitForce);

			Instantiate(weapon.hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

			hitInfo.collider.BroadcastMessage(
				"TakeDamage",
				weapon.hitDamage,
				SendMessageOptions.DontRequireReceiver);
		}

	}
}
