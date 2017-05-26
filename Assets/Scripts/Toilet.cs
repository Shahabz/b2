using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : MonoBehaviour {

	[SerializeField]
	ParticleSystem thisPiss;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			GetComponent<AudioSource> ().Play ();
			thisPiss.Play ();
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			GetComponent<AudioSource> ().Stop ();
			thisPiss.Stop ();
		}
	}
}
