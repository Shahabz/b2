using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackMatterHallway : MonoBehaviour {

	public bool isPlayerInHallway;
	bool isPlayerRunning;
	float spawnTime = .3f, spawnTimer;

//	float distanceBetweenLethalObjects = 2.5f;

	[SerializeField]
	GameObject blackMatterObject;

	//once the player stops running the walls retract, however if a wall is half way retracted and the player starts running again it will go back down
//	int ForeMostLethalObjectSpawned = 0; //counter: how to keep track of which lethal object currently exists in front
//	List<GameObject> ListOfLethalObjects = new List<GameObject>();
	PlayerInput playerInputReference;

	[SerializeField]
	Transform startTransform;
	[SerializeField]
	Transform endTransform;

	ParticleSystem fallingMatter;
	ParticleSystem fillMatter;

//	bool hasPriorSpawnCompleted;

	[SerializeField]
	float currentPosition;
	[SerializeField]
	float fillSpeed = 5f;

	void Start () {
		playerInputReference = TestPlayerController.s_instance.gameObject.GetComponent<PlayerInput> ();
		//fallingMatter = blackMatterObject.transform.Find("FallingMatter").GetComponent<ParticleSystem>();
		//fillMatter = blackMatterObject.transform.Find("FillMatter").GetComponent<ParticleSystem>();

//		ParticleSystem.EmissionModule emission = fallingMatter.emission;
		//emission.rateOverTime = 0f;
//		fallingMatter.Clear();
		//ParticleSystem.EmissionModule fillEmission = fillMatter.emission;
		//fillEmission.rateOverTime = 0f;
//		fillMatter.Clear();
	}
	
	void Update () {
		if (isPlayerInHallway) {
			HandleLethalObjectAnimation ();
		} else {
//			RetractLethalObjects ();
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			isPlayerInHallway = true;
		}

	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			isPlayerInHallway = false;
		}
	}

	void HandleLethalObjectAnimation() {
//		if (GenericTimer.RunGenericTimer (spawnTime, ref spawnTimer)) {
			if (playerInputReference.sprint) {
				SpawnLethalObject ();
			} else {
				RetractLethalObjects ();
			}
//		}
	}

	void RetractLethalObjects () {
//		if (ListOfLethalObjects.Count > 0) {
//			ListOfLethalObjects [ForeMostLethalObjectSpawned - 1].GetComponent<LethalObject> ().RetractObject ();
//			ListOfLethalObjects.Remove (ListOfLethalObjects [ForeMostLethalObjectSpawned - 1]);
//			ForeMostLethalObjectSpawned--;
//		}
		if(fallingMatter.isEmitting) {
			ParticleSystem.EmissionModule emission = fallingMatter.emission;
			emission.rateOverTime = 0f;
		}

		currentPosition -= Time.deltaTime / fillSpeed;
		blackMatterObject.transform.position = Vector3.Lerp(startTransform.position, endTransform.position, currentPosition);
		if(currentPosition <= 0f) {
			ParticleSystem.EmissionModule fillEmission = fillMatter.emission;
			fillEmission.rateOverTime = 0f;
		}
	}


	void SpawnLethalObject() {
		ParticleSystem.EmissionModule fallingEmission = fallingMatter.emission;
		if(fallingEmission.rateOverTime.constant == 0) {
			ParticleSystem.EmissionModule fillEmission = fillMatter.emission;
			fillEmission.rateOverTime = 800f;
			fallingEmission.rateOverTime = 400f;
		}

		currentPosition += Time.deltaTime / fillSpeed;
		blackMatterObject.transform.position = Vector3.Lerp(startTransform.position, endTransform.position, currentPosition);
//		Vector3 spawnPosition = spawnTransform.position + spawnTransform.forward * distanceBetweenLethalObjects * ForeMostLethalObjectSpawned;
//		GameObject tempLO = (GameObject)Instantiate (lethalObjectPrefab, spawnPosition, spawnTransform.rotation) as GameObject;
//		ListOfLethalObjects.Add (tempLO);
//		ForeMostLethalObjectSpawned++;
	}
}
