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
	[SerializeField] Transform davidTransform;

	TherapistState thisTherapistState;

	public static Therapist s_instance; 

	// Use this for initialization
	void Awake () {
		if (s_instance == null) {
			s_instance = this;
		} else {
			Destroy (this);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			PlayerController.s_instance.isNearTherapist = true;
			tolsoyAnimator.SetTrigger ("sitdown");
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			PlayerController.s_instance.isNearTherapist = false;
			tolsoyAnimator.SetTrigger ("standup");

		}

	}

	public void StartTherapistSession () {
		MultipleChoiceCameraOn ();
		PlayerController.s_instance.transform.position = davidTransform.position;
		PlayerController.s_instance.transform.rotation = davidTransform.rotation;

	}

}
