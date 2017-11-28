using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterOnExitTrigger : MonoBehaviour {
	public Light thisLight;
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			thisLight.intensity = 2f;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			thisLight.intensity = 1f;
		}

	}
}
