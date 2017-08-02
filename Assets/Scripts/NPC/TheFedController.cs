using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.ImageEffects;


public class TheFedController : MonoBehaviour {

	NavMeshAgent thisNavMeshAgent;


	// Use this for initialization
	void Start () {
		thisNavMeshAgent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			TestPlayerController.s_instance.GetComponent<HealthHandler> ().TakeStress (25);
			SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.Fed_AnxietyLightning);
			SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.Player_HeartBeatFast);
			SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.AnxietyHit1);
			StartCoroutine (ShowPSTDFX ());
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Player") {
			StartCoroutine (ShowPSTDFX2 ());
			TestPlayerController.s_instance.GetComponent<HealthHandler> ().TakeStress (100);
			SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.AnxietyHit2);
			SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.uruhit2);
		}
	}

	IEnumerator ShowPSTDFX () {
		Camera.main.GetComponent<Blur> ().enabled = true;
		Camera.main.GetComponent<Fisheye> ().enabled = true;
		yield return new WaitForSeconds (5f);
		Camera.main.GetComponent<Blur> ().enabled = false;
		Camera.main.GetComponent<Fisheye> ().enabled = false;
		SoundtrackManager.s_instance.Fed_AnxietyLightning.Stop ();

	}
	IEnumerator ShowPSTDFX2 () {
		Camera.main.GetComponent<Blur> ().enabled = true;
		Camera.main.GetComponent<Fisheye> ().enabled = true;
		Camera.main.GetComponent<ColorCorrectionCurves> ().enabled = true;
		yield return new WaitForSeconds (3f);
		Camera.main.GetComponent<Blur> ().enabled = false;
		Camera.main.GetComponent<Fisheye> ().enabled = false;
		Camera.main.GetComponent<ColorCorrectionCurves> ().enabled = false;
		SoundtrackManager.s_instance.Fed_AnxietyLightning.Stop ();

	}
}
