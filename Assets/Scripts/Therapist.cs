using UnityEngine;
using System.Collections;
using UnityEngine.UI;

enum TherapistState {Idle, Introduction, AskingQuestion, DavidSelectAnswer, SayingAnswer}

public class Therapist : MultipleChoice {

	//Show text only on questions and answers, small in between dialogue is audio only.

	//STATE MACHINE SWITCHES
	bool switchToIdle,switchToIntroduction,switchToAskingQuestion, switchToDavidSelectAnswer, switchToSayingAnswer;
	//STATE MACHINE TIMERS
	float introductionTimer, askingQuestionTimer;
	float introductionTime = 1f, askingQuestionTime = 1f;
	bool hasAskedQuestion;

	const string therapyAudioDirectory = "Audio/Therapy/";
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

	AudioClip currentAudioClip;

	[SerializeField]
	TherapySession[] allTherapySessions;
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
			//Welcome to therapy David

			if (switchToIntroduction) {
				switchToIntroduction = false;
				thisTherapistState = TherapistState.Introduction;
				welcomeToTherapyDavidLuna[GameManager.s_instance.day].Play ();
			}
			break;


		case TherapistState.Introduction:
			if (GenericTimer.RunGenericTimer (introductionTime + welcomeToTherapyDavidLuna[GameManager.s_instance.day].clip.length, ref introductionTimer)){
				thisTherapistState = TherapistState.AskingQuestion;
				therapistSubtitle.gameObject.SetActive (true);
				therapistSubtitle.text = currentTherapySession.therapySessionElements [questionIndex].questionString;
				currentAudioClip = Resources.Load(therapyAudioDirectory + currentTherapySession.therapySessionElements [questionIndex].questionAudioPath) as AudioClip;
				GetComponent<AudioSource> ().clip = currentAudioClip;
				GetComponent<AudioSource> ().Play ();
			}

			break;
		case TherapistState.AskingQuestion:
			if (GenericTimer.RunGenericTimer (askingQuestionTime + currentAudioClip.length, ref askingQuestionTimer)) {
				therapistSubtitle.gameObject.SetActive (false);
				answerPanel.SetActive (true);
				choiceA.text = currentTherapySession.therapySessionElements [questionIndex].answerChoices [0];
				choiceB.text = currentTherapySession.therapySessionElements [questionIndex].answerChoices [1];
				choiceC.text = currentTherapySession.therapySessionElements [questionIndex].answerChoices [2];
				choiceD.text = currentTherapySession.therapySessionElements [questionIndex].answerChoices [3];
				thisTherapistState = TherapistState.DavidSelectAnswer;
			}

			break;
		case TherapistState.DavidSelectAnswer:
			
			break;
		}
	}



}
