using UnityEngine;
using System.Collections;
using UnityEngine.UI;

enum TherapistState {Idle, Introduction, AskingQuestion, WaitingForAnswer, SayingAnswer}

public class Therapist : MultipleChoice {

	//it would be cool to fade into an abstract space when questions are being asked, or perhaps select 
	[SerializeField]
	Camera davidSitting, intro, therapistSitting, twoShot, selectAnswer;

	[SerializeField] Text therapistSubtitle, choiceA, choiceB, choiceC, choiceD;

	[SerializeField] Animator tolsoyAnimator;

	TherapistState thisTherapistState;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			PlayerController.s_instance.isNearTherapist = true;
			tolsoyAnimator.SetTrigger ("standup");
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			PlayerController.s_instance.isNearTherapist = false;
			tolsoyAnimator.SetTrigger ("sitdown");

		}

	}

	public void StartTherapistSession () {
		MultipleChoiceCameraOn ();
	}

}
