using UnityEngine;
using System.Collections;

public class WeaponEditor : MonoBehaviour {
	public static WeaponEditor instance;

	public Weapon[] weapons;
	public Clip[] clips;

	public int selectedWeaponIndex;

	void Awake () {
		instance = this;
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = Color.red;

		for (int orientationIndex=0; orientationIndex<4; orientationIndex++) {
			Gizmos.DrawWireSphere(
				weapons[selectedWeaponIndex].GetFirePosition(orientationIndex),
				0.2f);
			Gizmos.DrawRay (
				weapons[selectedWeaponIndex].GetFirePosition(orientationIndex),
				Orientation.GetDirection(orientationIndex));
		}
	}
}
