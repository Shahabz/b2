using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalHallway : MonoBehaviour {

	public bool isPlayerInHallway;
	bool isPlayerRunning;
	float spawnTime = .3f, spawnTimer;

	float distanceBetweenLethalObjects = 2.5f;

	[SerializeField]
	GameObject lethalObjectPrefab;

	//once the player stops running the walls retract, however if a wall is half way retracted and the player starts running again it will go back down
	int ForeMostLethalObjectSpawned = 0; //counter: how to keep track of which lethal object currently exists in front
	List<GameObject> ListOfLethalObjects = new List<GameObject>();
	PlayerInput playerInputReference;

	[SerializeField]
	Transform spawnTransform;

	bool hasPriorSpawnCompleted;

	void Start () {
		playerInputReference = TestPlayerController.s_instance.gameObject.GetComponent<PlayerInput> ();

	}
	
	void Update () {
		if (isPlayerInHallway) {
			HandleLethalObjectAnimation ();
		} else {
			RetractLethalObjects ();
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
		if (GenericTimer.RunGenericTimer (spawnTime, ref spawnTimer)) {
			if (playerInputReference.sprint) {
				SpawnLethalObject ();
			} else {
				RetractLethalObjects ();
			}
		}
	}

	void RetractLethalObjects () {
		if (ListOfLethalObjects.Count > 0) {
			ListOfLethalObjects [ForeMostLethalObjectSpawned - 1].GetComponent<LethalObject> ().RetractObject ();
			ListOfLethalObjects.Remove (ListOfLethalObjects [ForeMostLethalObjectSpawned - 1]);
			ForeMostLethalObjectSpawned--;
		}

	}


	void SpawnLethalObject() {
		Vector3 spawnPosition = spawnTransform.position + spawnTransform.forward * distanceBetweenLethalObjects * ForeMostLethalObjectSpawned;
		GameObject tempLO = (GameObject)Instantiate (lethalObjectPrefab, spawnPosition, spawnTransform.rotation) as GameObject;
		ListOfLethalObjects.Add (tempLO);
		ForeMostLethalObjectSpawned++;
	}
}
