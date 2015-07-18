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


	bool[] GetWeaponAvailable () {
		bool[] runtimeWeaponAvailable = new bool[WeaponEditor.weaponCount];

		for (int i = 0; i < runtimeWeaponAvailable.Length; i++) {
			if (weaponAvailable[i] && WeaponInventory.weaponCounts[i] > 0) {
				runtimeWeaponAvailable[i] = true;
			}
		}

		return runtimeWeaponAvailable;
	}

	bool WeaponAvailable (int weaponIndex) {
		return weaponAvailable[weaponIndex] && WeaponInventory.weaponCounts[weaponIndex] > 0;
	}


	public bool ChangeWeapon (int newWeaponIndex) {
		if (WeaponAvailable(newWeaponIndex)) {
			currentWeaponIndex = newWeaponIndex;
			return true;
		}
		return false;
	}

	public void NextWeapon () {
		for (int weaponIndex = currentWeaponIndex; weaponIndex < WeaponEditor.weaponCount; weaponIndex++) {
			if (ChangeWeapon(weaponIndex)) {
				return;
			}
		}

		for (int weaponIndex = 0; weaponIndex < WeaponEditor.weaponCount; weaponIndex++) {
			if (ChangeWeapon(weaponIndex)) {
				return;
			}
		}
	}

	public bool Reload () {
		return WeaponBehaviorController.Reload(currentWeaponIndex);
	}

	public bool Fire (int orientationIndex) {
		return WeaponBehaviorController.Fire(currentWeaponIndex, orientationIndex, transform);
	}
}
