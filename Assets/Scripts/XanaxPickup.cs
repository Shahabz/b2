using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XanaxPickup : MonoBehaviour, IInteractable {
	bool hasBeenPickedUp;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Interact () {
		if (!hasBeenPickedUp) {
			TestPlayerController.s_instance.GrabAndSwallowPills (gameObject);
			hasBeenPickedUp = true;
		}
	}
}
