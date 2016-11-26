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
    bool isDestroyed;
	public bool appliedOnThisComputerToday;
	bool isComputerBeingUsed;
	// Use this for initialization

	//The computer has a state machine and so does the player - this may be bad practice.


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
                PlayerController.s_instance.TryPracticeProgramming();
			break;
		case 1:
			if (!appliedOnThisComputerToday) {
				appliedOnThisComputerToday = true;
				PlayerController.s_instance.switchToApplyToJob = true;
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
		
   

    public void PunchComputer() {
        isDestroyed = true;
        TurnOff();
        PlayerController.s_instance.switchToWalking = true;
        //play vfx
    }

	public void UpdateJobState () {
		if (appliedOnThisComputerToday) {
			jobOption.text = "No Jobs Available";
		}
	}

}
