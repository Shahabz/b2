using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;

public class Computer : MultipleChoice {

	[SerializeField]
	GameObject projectedScreen;
	[SerializeField]
	Light computerLight;
	InputDevice inputDevice;


	[SerializeField]
	Text jobOption;

	bool appliedOnThisComputerToday;
	float programmingPracticeTime = 3.5f, programmingPracticeTimer, jobResponseTime = 3.5f, jobResponseTimer,showJobResponseTime, showJobResponseTimer;
	bool isPracticingProgramming, isApplyingToJob;
	bool isComputerBeingUsed;
	// Use this for initialization

	//The computer has a state machine and so does the player - this may be bad practice.

	// Update is called once per frame
	void Update () {
		if (isPracticingProgramming) {
			ShowProgrammingPractice ();
		}
		if (isApplyingToJob) {
			ShowJobResponse ();
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

	public override void SelectItem () {
		switch (selection) {
		case 0:
			PlayerController.s_instance.GetComponentInChildren<CodeThoughts> ().StartSpawning ();
			PlayerController.s_instance.switchToPracticeProgramming = true;
			isPracticingProgramming = true;
			break;
		case 1:
			if (!appliedOnThisComputerToday) {
				appliedOnThisComputerToday = true;
				PlayerController.s_instance.switchToApplyToJob = true;
				showJobResponseTime = JobText.s_instance.GetJobDescriptionScrollTime ();
				StartCoroutine ("WaitToShowJob");
			}

			break;
		case 2:
			
			break;
		case 3:
			TurnOff ();
			PlayerController.s_instance.switchToWalking = true;
			break;
		}
	}

	public void TurnOn () {
		projectedScreen.SetActive (true);
		MultipleChoiceCameraOn ();
		isActive = true;
	}

	public void TurnOff () {
		projectedScreen.SetActive (false);
		MultipleChoiceCameraOff ();
		isActive = false;
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

	void UpdateJobState () {
		if (appliedOnThisComputerToday) {
			jobOption.text = "No Jobs Available";
		}
	}

	IEnumerator WaitToShowJob () {
		yield return new WaitForSeconds (3f);
		isApplyingToJob = true;
		JobText.s_instance.SpawnJobText ();
	}

	void ShowJobResponse () {
		//wait for response
	
		//show response
		if (showJobResponseTimer < showJobResponseTime) {
			showJobResponseTimer += Time.deltaTime;

		}
		else {
			programmingPracticeTimer = 0;
			isApplyingToJob = false;
			appliedOnThisComputerToday = true;
			PlayerController.s_instance.switchToComputer = true;
			UpdateJobState ();
		}
	}
}
