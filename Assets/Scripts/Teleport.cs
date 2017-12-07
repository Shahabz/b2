using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
	public Transform teleportEnd;
	Vector3 offset;
	public bool alterRotation;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			Vector3 camoffset = Camera.main.transform.position - TestPlayerController.s_instance.transform.position;

			GetComponent<CinemachineHardCut> ().tempDisable ();
			offset = TestPlayerController.s_instance.transform.position - transform.position;
			TestPlayerController.s_instance.transform.position = teleportEnd.transform.position + offset;
			Camera.main.transform.position = TestPlayerController.s_instance.transform.position + camoffset;
			if (alterRotation)  TestPlayerController.s_instance.transform.rotation = teleportEnd.transform.rotation;
		}
	}
}
