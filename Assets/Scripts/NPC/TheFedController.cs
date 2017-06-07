using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheFedController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			TestPlayerController.s_instance.GetComponent<HealthHandler> ().TakeStress (80);
		}
	}
}
