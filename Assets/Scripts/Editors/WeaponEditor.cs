using UnityEngine;
using System.Collections;

/// <summary>
/// Holds informations and behaviors of weapons.
/// </summary>
public class WeaponEditor : MonoBehaviour {
	/// <summary>
	/// Provide Awake time access to the weapons.Length;
	/// Need to be set manually!!!
	/// </summary>
	public const int weaponCount = 2;
	/// <summary>
	/// Provide Awake time access to the clips.Length;
	/// Need to be set manually!!!
	/// </summary>
	public const int clipCount = 2;

	/// <summary>
	/// The runtime reference of the Weapons;
	/// The sequence in the array determine the weaponIndex.
	/// </summary>
	public static Weapon[] weapons;
	/// <summary>
	/// The runtime reference of the Clips;
	/// The sequence in the array determine the clipIndex that each weapon holds.
	/// </summary>
	public static Clip[] clips;

	public Weapon[] Weapons;
	public Clip[] Clips;


	void Awake () {
		AssignIndexes();

		StatizeObjects();
	}

	void AssignIndexes () {
		for (int i = 0; i < Weapons.Length; i++) {
			Weapons[i].index = i;
		}

		for (int i = 0; i < Clips.Length; i++) {
			Clips[i].index = i;
		}
	}

	void StatizeObjects () {
		weapons = Weapons;
		clips = Clips;
	}

	public static int GetClipIndex (int weaponIndex) {
		return weapons[weaponIndex].clipIndex;
	}


	// Show weapon's fire position in the SceneView.
	public int selectedWeaponIndex;
	void OnDrawGizmosSelected () {
		Gizmos.color = Color.red;

		for (int orientationIndex=0; orientationIndex<4; orientationIndex++) {
			Gizmos.DrawWireSphere(
				Weapons[selectedWeaponIndex].GetFirePosition(orientationIndex),
				0.2f);
			Gizmos.DrawRay (
				Weapons[selectedWeaponIndex].GetFirePosition(orientationIndex),
				Orientation.GetDirection(orientationIndex));
		}
	}
}
