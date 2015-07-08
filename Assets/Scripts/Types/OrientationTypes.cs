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

public class OrientationIndex {
	public float tolerance = 0.01f;

	int orientationIndex;

	public int GetOrientationIndex () {
		return orientationIndex;
	}

	public int RefreshOrientationIndex (Vector2 direction) {
		if (Mathf.Abs(Mathf.Abs(direction.x) - Mathf.Abs(direction.y)) < tolerance) {
			return orientationIndex;
		}
		
		bool positiveY = direction.y > 0;
		bool biggerX = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
		if (direction.x > 0) {
			if (positiveY) {
				// ^>
				if (biggerX) {
					orientationIndex = Orientation.right;
				} else {
					orientationIndex = Orientation.back;
				}
			} else {
				// v>
				if (biggerX) {
					orientationIndex = Orientation.right;
				} else {
					orientationIndex = Orientation.front;
				}
			}
		} else {
			if (positiveY) {
				//<^
				if (biggerX) {
					orientationIndex = Orientation.left;
				} else {
					orientationIndex = Orientation.back;
				}
			} else {
				//<v
				if (biggerX) {
					orientationIndex = Orientation.left;
				} else {
					orientationIndex = Orientation.front;
				}
			}
		}
		
		return orientationIndex;
	}
}
