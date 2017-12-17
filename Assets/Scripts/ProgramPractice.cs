using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramPractice : MonoBehaviour {
	bool isProgramming;
	float timer, time = 5f;
	int counter;
	public Transform spawnhere;
	public AudioSource regularProgramming, sexProgramming;
	public GameObject Explosion;
	public GameObject[] DestroyThese;
	public Cinemachine.CinemachineVirtualCamera thisCamera;
	// Use this for initialization
	public void StartProgramming () {

		if (!isProgramming) {
			if (counter < 2) {
				isProgramming = true;
				GetComponent<CodeThoughts> ().StartSpawning (false);
				thisCamera.enabled = true;
				regularProgramming.Play ();
				counter++;
			} else {
				regularProgramming.Play ();
				sexProgramming.Play ();
				isProgramming = true;
				GetComponent<CodeThoughts> ().StartSpawning (true);
				thisCamera.enabled = true;
				counter++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isProgramming)
			timer += Time.deltaTime;
		if (timer > time)
			StopProgramming ();
	}

	void StopProgramming ()
	{
		GetComponent<CodeThoughts> ().StopSpawning ();
		isProgramming = false;
		thisCamera.enabled = false;
		timer = 0;
		if (counter > 2)
			DestroyComputer ();
	}

	void DestroyComputer ()
	{
		Instantiate (Explosion,transform.parent);
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.catkill1);
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.catkill2);
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.catkill3);

		TestPlayerController.s_instance.GetComponent<HealthHandler> ().TakeStress (10);
		TestPlayerController.s_instance.GetComponent<Animator> ().SetTrigger ("punch");
		foreach (GameObject x in DestroyThese) {
			Destroy (x);
		}
	}
}
