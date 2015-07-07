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
	bool usingWeapon;

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

	public void SetUsingWeapon (bool newUsingWeapon) {
		usingWeapon = newUsingWeapon;
	}

	public bool Reload () {
		if (!usingWeapon) {
			return false;
		}

		return WeaponBehaviorController.Reload(currentWeaponIndex);
	}

	public bool Fire () {
		if (!usingWeapon) {
			return false;
		}

		return WeaponBehaviorController.Fire(currentWeaponIndex);
	}
}
