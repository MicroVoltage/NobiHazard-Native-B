using UnityEngine;
using System.Collections;

/// <summary>
/// The commander of the PlayerController
/// </summary>
[RequireComponent(typeof(PlayerController))]
public class PlayerInputController : MonoBehaviour {
	float tolerance = 0.01f;

	public bool suspended;

	PlayerController playerController;

	void Start () {
		playerController = GetComponent<PlayerController>();
	}

	void Update () {
		if (suspended) {
			return;
		}

		// Get input data
		Vector2 direction = new Vector2();
		direction.x = Input.GetAxis("H");
		direction.y = Input.GetAxis("V");

		bool jump = Input.GetButton("J");
		bool fire = Input.GetButton("F");
		bool reload = Input.GetButton("R");
		bool nextWeapon = Input.GetButton("N");

		// Activate the controller
		if (direction.magnitude > tolerance) {
			playerController.Move(direction);
		} else {
			playerController.Stop();
		}
		if (jump) {
			playerController.Jump();
		}
		if (fire) {
			playerController.Fire();
		}		
		if (reload) {
			playerController.Reload();
		}
		if (nextWeapon) {
			playerController.NextWeapon();
		}
	}
}
