using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class GenericMovementController : MonoBehaviour {
	float moveForce;
	Vector3 direction;

	new Rigidbody rigidbody;

	float tolerance = 0.01f;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();
	}
	
	public void SetMoveForce(float newMoveForce) {
		moveForce = newMoveForce;
	}

	public void SetDirection (Vector3 newDirection) {
		direction = newDirection;
	}

	public void Stop () {
		moveForce = 0.0f;
		direction = Vector3.zero;
	}

	void FixedUpdate () {
		if (CalculateMoveForce().magnitude < tolerance) {
			return;
		}

		rigidbody.AddForce(CalculateMoveForce() * Time.fixedDeltaTime);
	}

	Vector3 CalculateMoveForce () {
		return direction.normalized * moveForce;
	}
}
