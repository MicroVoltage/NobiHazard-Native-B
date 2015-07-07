using UnityEngine;
using System.Collections;

public class Weapon : Holdable {
	public string animationName;
	
	public int clipIndex;

	public int tragectoryCount;
	public FirePercision firePercision;
	public bool penetrating;

	public Vector3 upwardFirePosition;
	public Vector3 downwardFirePosition;
	public Vector3 leftwardFirePosition;
	public Vector3 rightwardFirePosition;
	
	public float recoilForce;
	public float hitForce;
	public float hitDamage;
	
	public GameObject fireEffect;
	public GameObject hitEffect;

	/// <summary>
	/// If (bullet), use hitForce to launch bullet(s).
	/// </summary>
	public GameObject bullet;


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

public class Clip : Holdable {
	public int ammoCount;
}

public class FirePercision {
	public float hitShpereRadius;
	public float hitSphereDistance;

	public Vector3 GetRandomFireDirection (Vector3 fireDirection) {
		return fireDirection * hitSphereDistance + Random.insideUnitSphere * hitShpereRadius;
	}
}

public class WeaponState {
	public static string[] strings = {"draw", "idle", "walk", "fire"};
	
	public static int draw = 0;
	public static int idle = 1;
	public static int walk = 2;
	public static int fire = 3;
}