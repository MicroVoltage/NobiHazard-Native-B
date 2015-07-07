using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GenericMovementController))]
public class CharacterMovementController : MonoBehaviour {
	GenericMovementController genericMovementController;
	new Rigidbody rigidbody = new Rigidbody();

	void Start () {
		genericMovementController = GetComponent<GenericMovementController>();
		rigidbody = GetComponent<Rigidbody>();
	}

	public void Move (Vector2 force) {
		genericMovementController.SetMoveForce(force.magnitude);

		genericMovementController.SetDirection(V2ToV3(force));
	}

	public void Stop () {
		genericMovementController.Stop();
	}

	public void Jump (float magnitude) {
		rigidbody.AddForce(Vector3.up * magnitude);
	}

	public void Rush (Vector2 force) {
		rigidbody.AddForce(V2ToV3(force));
	}

	Vector3 V2ToV3 (Vector2 v2) {
		return new Vector3(v2.x, 0, v2.y);
	}
}
