using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractEvent : MonoBehaviour, IInteractable {

	public UnityEvent OnInteractEvents;
	bool hasPlayed;
	public bool infinitelyTriggerable;
	public void Interact() {
		if (!hasPlayed)
			OnInteractEvents.Invoke ();
		if (!infinitelyTriggerable)
			hasPlayed = true;
		
	}
}
