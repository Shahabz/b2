using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTriggerEvent : MonoBehaviour {
	public AudioSource[] OnEventTriggerSounds;
	public AudioSource[] OnEventStopSounds;
	public GameObject[]  DisableThisObject;
	public GameObject[] EnableThisObject;
	public ParticleSystem PlayThisParticle;
	public ParticleSystem[] PlayTheseParticles;
	bool hasPlayed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && !hasPlayed) {
			hasPlayed = true;
			foreach (AudioSource x in OnEventTriggerSounds) {
				x.Play ();
			}
			foreach (AudioSource x in OnEventStopSounds) {
				x.Stop ();
			}
			foreach (GameObject x in DisableThisObject) {
				x.SetActive (false);
			}
			foreach (GameObject x in EnableThisObject) {
				x.SetActive (true);
			}
			foreach (ParticleSystem x in PlayTheseParticles) {
				x.gameObject.SetActive (true);
				x.Play ();
			}
			if (PlayThisParticle!=null)
				PlayThisParticle.Play ();
		}
	}
}
