using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class PlayerInputController : MonoBehaviour {
	float tolerance = 0.01f;

	bool isSuspended;

	PlayerController playerController;

	void Start () {
		playerController = GetComponent<PlayerController>();
	}

	void Update () {
		if (isSuspended) {
			return;
		}

		Vector2 direction = new Vector2();
		direction.x = Input.GetAxis("H");
		direction.y = Input.GetAxis("V");

		bool jump = Input.GetButton("J");

		if (direction.magnitude > tolerance) {
			playerController.Move(direction);
		} else {
			playerController.Stop();
		}

		if (jump) {
			playerController.Jump();
		}
	}

	public void Suspend () {
		isSuspended = true;
	}

	public void Resume () {
		isSuspended = false;
	}
}
