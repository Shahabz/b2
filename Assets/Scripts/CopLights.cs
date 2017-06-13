using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopLights : MonoBehaviour {

	public GameObject[] redLights, blueLights;
	float flashTime = 1f, flashTimer;
	// Use this for initialization
	void Start () {
		foreach (GameObject x in redLights) {
			x.SetActive (true);
		}
		foreach (GameObject x in blueLights) {
			x.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (GenericTimer.RunGenericTimer (flashTime, ref flashTimer)) {
			AlternateLights ();
		}
	}

	void AlternateLights() {
		foreach (GameObject x in redLights) {
			x.SetActive (!x.activeSelf);
		}
		foreach (GameObject x in blueLights) {
			x.SetActive (!x.activeSelf);
		}
	}
}
