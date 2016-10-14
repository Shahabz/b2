using UnityEngine;
using System.Collections;

public class Therapist : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			PlayerController.s_instance.isNearTherapist = true;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			PlayerController.s_instance.isNearTherapist = false;
		}

	}

}
