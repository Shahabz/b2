using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrobeLight : MonoBehaviour {

	float flashTime = .34f, flashTimer;
	// Use this for initialization
	Light thisLight;
	void Start () {
		thisLight = GetComponent<Light> ();
	}

	// Update is called once per frame
	void Update () {
		
		if (GenericTimer.RunGenericTimer (flashTime, ref flashTimer)) {
			thisLight.enabled = !thisLight.isActiveAndEnabled;
			StartCoroutine ("Off");
		}

	}
	IEnumerator Off(){
		yield return new WaitForSeconds (.05f);
		thisLight.enabled = false;
	}
}

