using UnityEngine;
using System.Collections;
using UnityEngine.UI;

enum TherapistState {Idle, Introduction, AskingQuestion, DavidSelectAnswer, SayingAnswer}

public class Therapist : MultipleChoice {

	bool switchToIdle,switchToIntroduction,switchToAskingQuestion, switchToDavidSelectAnswer, switchToSayingAnswer;
	bool hasAskedQuestion;
	//it would be cool to fade into an abstract space when questions are being asked, or perhaps select 
	[SerializeField]
	Camera davidSitting, OTS_TtoD, OTS_DtoT, twoShot, selectAnswer, topDown;

	[SerializeField]
	GameObject answerPanel;

	[SerializeField] Text therapistSubtitle, choiceA, choiceB, choiceC, choiceD;

	[SerializeField] Animator tolsoyAnimator;
	[SerializeField] Transform davidTransform;

	TherapistState thisTherapistState;

	public AudioSource[] incorrectBark, correctBark, welcomeToTherapyDavidLuna;

	[SerializeField]
	TherapySession[] allTherapySession;
	TherapySession currentTherapySession; 

	int questionIndex = 0;

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
		switchToIntroduction = true;


	}

	public void EndTherapistSession () {

	}

	void Update() {
		switch (thisTherapistState) {
		case TherapistState.Idle:

			if (switchToIntroduction) {
				switchToIntroduction = false;
				thisTherapistState = TherapistState.Introduction;
				welcomeToTherapyDavidLuna[GameManager.s_instance.day].Play ();
			}
			break;
		case TherapistState.AskingQuestion:
			if (welcomeToTherapyDavidLuna [GameManager.s_instance.day].isPlaying == false && hasAskedQuestion == false) {
				StartCoroutine ("AskQuestion");
				hasAskedQuestion = true;
			}
			if (switchToDavidSelectAnswer) {
				switchToDavidSelectAnswer = false;
				thisTherapistState = TherapistState.DavidSelectAnswer;
			}
			break;
		case TherapistState.DavidSelectAnswer:

			break;
		}
	}

	IEnumerator AskQuestion() {
		yield return new WaitForSeconds (1f);
		currentTherapySession.therapySessionElements [questionIndex].question.Play ();
		yield return new WaitForSeconds (currentTherapySession.therapySessionElements [questionIndex].question.clip.length);
		switchToDavidSelectAnswer = true;
	}
}
