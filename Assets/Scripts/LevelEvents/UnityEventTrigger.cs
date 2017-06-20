using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventTrigger : MonoBehaviour {

	public UnityEvent myUnityEvent;

	bool hasPlayed;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && !hasPlayed) {
			hasPlayed = true;
			if (myUnityEvent.GetPersistentTarget (0) != null) {
				myUnityEvent.Invoke ();
			}
		}
	}
}
