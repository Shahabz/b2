using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEventTrigger : UnityEventTrigger {

	public float timeUntilTriggered;
	float timer;
	bool isBeingTimed;
	// Use this for initialization
	void Start () {
		
	}

	protected override void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player" && !hasPlayed) 
		{
			isBeingTimed = true;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player" && !hasPlayed) {
			isBeingTimed = false;
		}
	}

	// Update is called once per frame
	void Update () {
		if (isBeingTimed)
			timer += Time.deltaTime;
		if (timer > timeUntilTriggered && !hasPlayed)
			ExecuteEvent ();
	}
}
