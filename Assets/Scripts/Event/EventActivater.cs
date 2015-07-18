using UnityEngine;
using System.Collections;

public class EventActivater : MonoBehaviour {
	public const string eventMessage = "OnEvent";

	public bool activeOnce;
	bool activated;

	public EventActivater nextEvent;


	public static void ActivateEvent (GameObject target) {
		if (target.activeInHierarchy) {
			target.GetComponent<EventActivater>().OnEventEnter();
		}
	}
 
	public void OnEventEnter () {
		if ((!activeOnce) || (!activated)) {
			gameObject.SendMessage(eventMessage, SendMessageOptions.RequireReceiver);

			activated = true;
		}
	}

	public void OnEventExit () {
		if (nextEvent != null) {
			nextEvent.OnEventEnter();
		}
	}
}
