using UnityEngine;
using System.Collections;

/// <summary>
/// Load/Save/Manage the weapon asociated numbers.
/// </summary>
[RequireComponent(typeof(WeaponEditor))]
public class WeaponInventory : MonoBehaviour {
	public const string initializedFlagKey = "WeaponInventoryInitialized";
	public const string weaponCountsKey = "weaponCounts";
	public const string clipCountsKey = "clipCountsKey";
	public const string ammoCountsKey = "ammoCountsKey";

	public static int[] weaponCounts;
	public static int[] clipCounts;
	public static int[] ammoCounts;

	void Awake () {
		LoadInventoryStates();
	}

	public static void SaveInventoryStates () {
		DataSL.SaveArray<int>(weaponCountsKey, weaponCounts);
		DataSL.SaveArray<int>(clipCountsKey, clipCounts);
		DataSL.SaveArray<int>(ammoCountsKey, ammoCounts);
	}

	public static void LoadInventoryStates () {
		if (!DataSL.LoadData<bool>(initializedFlagKey)) {
			InitializeInventory();
			return;
		}

		weaponCounts = DataSL.LoadArray<int>(weaponCountsKey);
		clipCounts = DataSL.LoadArray<int>(clipCountsKey);
		ammoCounts = DataSL.LoadArray<int>(ammoCountsKey);
	}

	static void InitializeInventory () {
		weaponCounts = new int[WeaponEditor.weaponCount];
		clipCounts = new int[WeaponEditor.clipCount];
		ammoCounts = new int[clipCounts.Length];

		SaveInventoryStates();

		DataSL.SaveData<bool>(initializedFlagKey, true);
	}

	public static void AddWeapon (int weaponIndex, int count) {
		if (count < 0) {
			Debug.LogError("Trying to add a minus value: " + count);
			return;
		}

		weaponCounts[weaponIndex] += count;
	}

	public static void AddClip (int weaponIndex, int count) {
		if (count < 0) {
			Debug.LogError("Trying to add a minus value: " + count);
			return;
		}
		
		clipCounts[weaponIndex] += count;
	}

	public static void SubWeapon (int weaponIndex, int count) {
		if (count < 0) {
			Debug.LogError("Trying to substract a minus value: " + count);
			return;
		}
		
		weaponCounts[weaponIndex] -= count;
	}
	
	public static void SubClip (int weaponIndex, int count) {
		if (count < 0) {
			Debug.LogError("Trying to substract a minus value: " + count);
			return;
		}
		
		clipCounts[weaponIndex] -= count;
	}

	/// <summary>
	/// The ONLY method that can add to the ammoCount;
	/// Return false if have no clip left.
	/// </summary>
	public static bool Reload (int weaponIndex) {
		int clipIndex = WeaponEditor.GetClipIndex(weaponIndex);
		if (clipCounts[clipIndex] <= 0) {
			return false;
		}

		clipCounts[clipIndex] --;
		ammoCounts[clipIndex] = WeaponEditor.clips[clipIndex].ammoCount;

		return true;
	}

	/// <summary>
	/// Substract 1 ammo from the ammoCount;
	/// Return false if have no ammo left.
	/// </summary>
	public static bool SubAmmo (int weaponIndex) {
		int clipIndex = WeaponEditor.GetClipIndex(weaponIndex);
		if (ammoCounts[clipIndex] <= 0) {
			return false;
		}
		
		ammoCounts[clipIndex] --;

		return true;
	}
}
