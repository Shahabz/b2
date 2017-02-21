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
    GameObject computerExplosion;
    [SerializeField]
    MeshRenderer computerScreen;

	[SerializeField]
	Text jobOption;
    bool isDestroyed;
	public bool appliedOnThisComputerToday;
	bool isComputerBeingUsed;

    // Use this for initialization

    //The computer has a state machine and so does the player - this may be bad practice.


    void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && !isDestroyed) {
			computerLight.intensity = 2f;
			PlayerController.s_instance.isNearComputer = true;
			PlayerController.s_instance.currentComputer = this;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player" && !isDestroyed) {
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
        CameraManager.s_instance.SetMainViewOnScene(mainViewOfMultipleChoice);
		CameraManager.s_instance.MultipleChoiceCameraOn ();
		isActive = true;
	}

	public void TurnOff () {
		projectedScreen.SetActive (false);
        CameraManager.s_instance.MultipleChoiceCameraOff ();
		isActive = false;
	}
		
   

    public void PunchComputer() {
        isDestroyed = true;
        TurnOff();
        PlayerController.s_instance.switchToWalking = true;
        //play vfx
        StartCoroutine("ExplodeComputer");

    }

    IEnumerator ExplodeComputer()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(computerExplosion, transform.position, Quaternion.identity);
        computerLight.intensity = 0f;
        PlayerController.s_instance.isNearComputer = false;
        PlayerController.s_instance.currentComputer = null;
        computerScreen.material.color = Color.black;

    }

    public void UpdateJobState () {
		if (appliedOnThisComputerToday) {
			jobOption.text = "No Jobs Available";
		}
	}

}
