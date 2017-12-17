using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterOnExitTrigger : MonoBehaviour {
	public Light thisLight;
	public AudioSource thisAudio;
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			if(thisLight)thisLight.intensity = 2f;
			if(thisAudio)thisAudio.Play();

		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			if(thisLight)thisLight.intensity = 1f;
			if(thisAudio)thisAudio.Stop();

		}

	}
}
