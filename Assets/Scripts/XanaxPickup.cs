using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XanaxPickup : MonoBehaviour, IInteractable {
	bool hasBeenPickedUp;
	public XanaxWaypoint owner;


	public void Interact () {
		if (!hasBeenPickedUp) {
			GetComponent<BoxCollider> ().enabled = false;
			TestPlayerController.s_instance.GrabAndSwallowPills (gameObject);
			hasBeenPickedUp = true;
			if (owner) {
				owner.hasBeenAccessed = true;
			}
		}
	}
}
