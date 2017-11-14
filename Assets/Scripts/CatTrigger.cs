using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatTrigger : TimedEventTrigger {

	// Use this for initialization
	void Start () {
		
	}
	protected override void OnTriggerEnter (Collider other)
	{
		if (other.tag == "CLIFFORD" && !hasPlayed && !notTriggeredByCollider) 
		{
			isBeingTimed = true;
		}
	}

	protected void OnTriggerExit (Collider other) {
		if (other.tag == "CLIFFORD" && !hasPlayed && !notTriggeredByCollider) {
			isBeingTimed = false;
		}
	}

	// Update is called once per frame
	void Update () {
		base.Update ();
	}
}
