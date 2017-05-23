using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalHallway : MonoBehaviour {

	bool isPlayerInHallway;
	bool isPlayerRunning;
	float spawnTime = .3f, spawnTimer;

	[SerializeField]
	float distanceBetweenLethalObjects;

	[SerializeField]
	GameObject lethalObjectPrefab;

	//once the player stops running the walls retract, however if a wall is half way retracted and the player starts running again it will go back down
	int ForeMostLethalObjectSpawned = 0; //counter: how to keep track of which lethal object currently exists in front
	List<GameObject> ListOfLethalObjects = new List<GameObject>();
	PlayerInput playerInputReference;

	bool hasPriorSpawnCompleted;

	// Use this for initialization
	void Start () {
		playerInputReference = TestPlayerController.s_instance.gameObject.GetComponent<PlayerInput> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (isPlayerInHallway) {
			HandleLethalObjectAnimation ();
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
		if (ListOfLethalObjects [ForeMostLethalObjectSpawned - 1] != null) {
			ListOfLethalObjects [ForeMostLethalObjectSpawned - 1].GetComponent<LethalObject> ().RetractObject ();
		}

	}


	void SpawnLethalObject() {


		Vector3 spawnPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z + distanceBetweenLethalObjects * ForeMostLethalObjectSpawned);
		Instantiate(lethalObjectPrefab, spawnPosition, transform.rotation);
		ForeMostLethalObjectSpawned++;
	}
}
