using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;

public class Computer : MonoBehaviour {

	public GameObject[] arrows;
	[SerializeField]
	GameObject projectedScreen;
	[SerializeField]
	Camera thisViewOfComputer;
	int selection = 0;
	[SerializeField]
	Light computerLight;
	InputDevice inputDevice;
	Vector3 lastCamPos;
	Quaternion lastCamRot;

	[SerializeField]
	Text[] selectionArrows;

	float programmingPracticeTime = 3.5f, programmingPracticeTimer;
	bool isPracticingProgramming;
	bool isComputerBeingUsed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isPracticingProgramming) {
			ShowProgrammingPractice ();
		}

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			computerLight.intensity = 2f;
			PlayerController.s_instance.isNearComputer = true;
			PlayerController.s_instance.currentComputer = this;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			computerLight.intensity = 1f;
			PlayerController.s_instance.isNearComputer = false;
			PlayerController.s_instance.currentComputer = null;
		}

	}

	public void SelectItem () {
		switch (selection) {
		case 0:
			PlayerController.s_instance.GetComponentInChildren<CodeThoughts> ().StartSpawning ();
			PlayerController.s_instance.switchToAnxietyCam = true;
			isPracticingProgramming = true;
			break;
		case 1:
			
			break;
		case 2:
			
			break;
		case 3:

			break;
		}
	}

	public void TurnOn () {
		projectedScreen.SetActive (true);
		lastCamPos = Camera.main.transform.position;
		lastCamRot = Camera.main.transform.rotation;
		ComputerCameraOn ();


	}

	public void TurnOff () {
		projectedScreen.SetActive (false);
	}

	public void ComputerCameraOn () {
		Camera.main.transform.rotation = thisViewOfComputer.transform.rotation;
		Camera.main.transform.position = thisViewOfComputer.transform.position;
	}
	void ComputerCameraOff() {
		Camera.main.transform.rotation = lastCamRot;
		Camera.main.transform.position = lastCamPos;
	}

	public void ArrowDown() {
		if (selection >= arrows.Length-1) {
			selection = 0;
		} else {
			selection++;
		}
		ShowSelection();
	}

	public void ArrowUp() {
		if (selection < 1) {
			selection = arrows.Length-1;
		} else {
			selection--;
		}
		ShowSelection();

	}
	void ShowSelection() {
		for (int i = 0; i < arrows.Length;i++) {
			if (selection == i) {
				arrows [i].SetActive(true);
			} else {
				arrows [i].SetActive(false);
			}
		}
	}

	void ShowProgrammingPractice () {
		//visual representation of coding
		if (programmingPracticeTimer < programmingPracticeTime) {
			programmingPracticeTimer += Time.deltaTime;
			return;
		} else {
			isPracticingProgramming = false;
			programmingPracticeTimer = 0;
			PlayerController.s_instance.ReceiveAnxiety ();
			PlayerController.s_instance.GetComponentInChildren<CodeThoughts> ().StopSpawning ();

		}
	}
}
