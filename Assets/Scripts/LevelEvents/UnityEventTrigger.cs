using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventTrigger : MonoBehaviour {

	public UnityEvent myUnityEvent;
	public bool isInfinitelyTriggerable;
	protected bool hasPlayed;

	protected virtual void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && !hasPlayed) {
			ExecuteEvent ();
		}
	}

	void Start() {
		if (GetComponent<MeshRenderer>() != null)
			GetComponent<MeshRenderer> ().enabled = false;
	}

	public void ExecuteEvent () {
		if (!isInfinitelyTriggerable)hasPlayed = true;
		if (myUnityEvent.GetPersistentTarget (0) != null) {
			myUnityEvent.Invoke ();
		}
	}
}
