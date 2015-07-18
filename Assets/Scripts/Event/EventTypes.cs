using UnityEngine;
using System.Collections;

public abstract class GenericEvent : MonoBehaviour {
	public abstract void OnEvent ();

	protected void ExitEvent () {
		gameObject.GetComponent<EventActivater>().OnEventExit();
	}
}