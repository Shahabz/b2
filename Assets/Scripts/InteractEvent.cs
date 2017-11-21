using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractEvent : MonoBehaviour, IInteractable {

	public UnityEvent OnInteractEvents;

	public void Interact() {
		OnInteractEvents.Invoke ();
	}
}
