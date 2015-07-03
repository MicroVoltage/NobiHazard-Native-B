using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class PlayerWeaponController : MonoBehaviour {
	public bool[] isWeaponAvailable;

	int weaponIndex;
	bool[] hasWeapon;
	int[] ammoCounts;
	int[] clipCounts;

	PlayerController playerController;

	void Start () {
		playerController = GetComponent<PlayerController>();

		hasWeapon = new bool[WeaponEditor.instance.weapons.Length];
		for (int weaponIndex=0; weaponIndex<hasWeapon.Length; weaponIndex++) {
			// load weapon data
		}

		clipCounts = new int[WeaponEditor.instance.clips.Length];
		for (int clipIndex=0; clipIndex<ammoCounts.Length; clipIndex++) {
			// load cilps data;
		}

		ammoCounts = new int[clipCounts.Length];
		for (int clipIndex=0; clipIndex<ammoCounts.Length; clipIndex++) {
			if (clipCounts[clipIndex] > 0) {
				ammoCounts[clipIndex] = WeaponEditor.instance.clips[clipIndex].ammoCount;
			}
		}
	}

	public void ChangeWeapon (int newWeaponIndex) {
		if (IsWeaponAvailable(newWeaponIndex) && hasWeapon[newWeaponIndex]) {
			weaponIndex = newWeaponIndex;
			playerController.ChangeWeapon(weaponIndex);
		}
	}
	
	public void Fire () {
		if (!HasAmmo(weaponIndex)) {
			return;
		}

		if (!HasAmmoInClip(weaponIndex)) {
			Reload();
		} else {
			playerController.Fire(weaponIndex);
			playerController.Recoil(WeaponEditor.instance.weapons[weaponIndex].recoilForce);
		}
	}

	public void Reload () {
		playerController.ReloadWeapon(weaponIndex);

		clipCounts[FindClipIndex(weaponIndex)]--;
		ammoCounts[FindClipIndex(weaponIndex)] = 
			WeaponEditor.instance.clips[FindClipIndex(weaponIndex)].ammoCount;
	}

	public void Disarm () {
		playerController.Disarm();
	}

	bool IsWeaponAvailable (int weaponIndex) { 
		return isWeaponAvailable[weaponIndex];
	}

	bool HasClip (int weaponIndex) {
		return clipCounts[FindClipIndex(weaponIndex)] > 0;
	}

	bool HasAmmoInClip (int weaponIndex) {
		return ammoCounts[FindClipIndex(weaponIndex)] > 0;
	}

	bool HasAmmo (int weaponIndex) {
		return HasClip (weaponIndex) || HasAmmoInClip(weaponIndex);
	}

	int FindClipIndex (int weaponIndex) {
		return WeaponEditor.instance.weapons[weaponIndex].clipIndex;
	}
}
