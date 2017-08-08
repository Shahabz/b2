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
	public bool slowDownTime;
	public bool doNotTriggerOnCollide = false;

	public void CallTriggerItems() {
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

		if (slowDownTime) {
			StartCoroutine ("SlowDownTime");
		}

		if (PlayThisParticle!=null)
			PlayThisParticle.Play ();

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && !hasPlayed && !doNotTriggerOnCollide) {
			CallTriggerItems ();
		}
	}

	IEnumerator SlowDownTime () {
		Time.timeScale = .5f;
		yield return new WaitForSeconds (.5f);
		Time.timeScale = 1f;
	}
}
