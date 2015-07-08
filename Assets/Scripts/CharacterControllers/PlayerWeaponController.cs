using UnityEngine;
using System.Collections;

/// <summary>
/// Interfacing the globle WeaponBehaviorController;
/// Controlled by the playerController!!!
/// </summary>
public class PlayerWeaponController : MonoBehaviour {
	public bool[] weaponAvailable;
	
	/// <summary>
	/// The index of the current weapon;
	/// Can ONLY be changed by ChangeWeapon(int weaponIndex).
	/// </summary>
	int currentWeaponIndex = -1;


	public bool[] GetWeaponAvailable () {
		bool[] runtimeWeaponAvailable = new bool[WeaponEditor.weaponCount];

		for (int i = 0; i < runtimeWeaponAvailable.Length; i++) {
			if (weaponAvailable[i] && WeaponInventory.weaponCounts[i] > 0) {
				runtimeWeaponAvailable[i] = true;
			}
		}

		return runtimeWeaponAvailable;
	}

	public void ChangeWeapon (int newWeaponIndex) {
		currentWeaponIndex = newWeaponIndex;
	}

	public bool Reload () {
		return WeaponBehaviorController.Reload(currentWeaponIndex);
	}

	public bool Fire (int orientationIndex) {
		return WeaponBehaviorController.Fire(currentWeaponIndex, orientationIndex, transform);
	}
}
