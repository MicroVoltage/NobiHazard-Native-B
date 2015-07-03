using UnityEngine;
using System.Collections;

public class Orientation {
	public static string[] strings = {"back", "front", "left", "right"};

	public const int back = 0;
	public const int front = 1;
	public const int left = 2;
	public const int right = 3;

	public static Vector3 GetDirection (int orientationIndex) {
		switch(orientationIndex) {
		case  Orientation.back:
			return Vector3.forward;
		case  Orientation.front:
			return Vector3.back;
		case  Orientation.left:
			return Vector3.left;
		case  Orientation.right:
			return Vector3.right;
		}

		Debug.LogError(orientationIndex + " - wrong orientation index");
		return Vector2.zero;
	}

	public static Quaternion GetRotation (int orientationIndex) {
		return Quaternion.LookRotation(GetDirection(orientationIndex));
	}
}

public class WeaponState {
	public static string[] strings = {"draw", "idle", "walk", "fire"};
	
	public static int draw = 0;
	public static int idle = 1;
	public static int walk = 2;
	public static int fire = 3;
}

[System.Serializable]
public class Item {
	public string name;
	public string description;

	public int index;
}

[System.Serializable]
public class Weapon : Item{
	public string animationName;

	public int clipIndex;

	public Vector3 upwardFirePosition;
	public Vector3 downwardFirePosition;
	public Vector3 leftwardFirePosition;
	public Vector3 rightwardFirePosition;
	public float recoilForce;
	public float hitForce;
	public float hitDamage;

	public GameObject fireEffect;
	public GameObject hitEffect;

	public Vector3 GetFirePosition (int orientationIndex) {
		switch(orientationIndex) {
		case  Orientation.back:
			return upwardFirePosition;
		case  Orientation.front:
			return downwardFirePosition;
		case  Orientation.left:
			return leftwardFirePosition;
		case  Orientation.right:
			return rightwardFirePosition;
		}

		Debug.LogError(orientationIndex + " - wrong orientation index");
		return Vector2.zero;
	}
}

[System.Serializable]
public class Clip : Item{
	public int ammoCount;
}