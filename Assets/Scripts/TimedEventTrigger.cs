using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEventTrigger : UnityEventTrigger {

	public float timeUntilTriggered;
	protected float timer;
	protected bool isBeingTimed;
	public bool notTriggeredByCollider;
	public bool autoStart;
	// Use this for initialization
	void Start () {
		if (autoStart)
			ForceStart ();
	}

	protected override void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player" && !hasPlayed && !notTriggeredByCollider) 
		{
			isBeingTimed = true;
		}
	}

	protected void OnTriggerExit (Collider other) {
		if (other.tag == "Player" && !hasPlayed && !notTriggeredByCollider) {
			isBeingTimed = false;
		}
	}

	// Update is called once per frame
	protected void Update () {
		if (isBeingTimed)
			timer += Time.deltaTime;
		if (timer > timeUntilTriggered && !hasPlayed)
			ExecuteEvent ();
	}

	public void ForceStart() {
		isBeingTimed = true;
	}
}
