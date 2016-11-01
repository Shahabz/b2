using UnityEngine;
using System.Collections;
using UnityEngine.UI;

enum TherapistState {Idle, Introduction, AskingQuestion, DavidSelectAnswer, AnxietyAnimation, TherapistWaitingToRespond, TherapistResponse, CheckForNextQuestion, EndSession};

public class Therapist : MultipleChoice {

	//Show text only on questions and answers, small in between dialogue is audio only.

	//STATE MACHINE SWITCHES
	public bool switchToIdle,switchToIntroduction,switchToAskingQuestion, switchToDavidSelectAnswer, switchToAnxietyAnimation, switchToTherapistWaitingToRespond;

	//STATE MACHINE TIMERS
	float introductionTimer, askingQuestionTimer, waitingTimer;
	float introductionTime = 1f, askingQuestionTime = 1f, waitingTime = 1;
	bool hasAskedQuestion;

	const string therapyAudioDirectory = "Audio/Therapy/";
	//it would be cool to fade into an abstract space when questions are being asked, or perhaps select 
	[SerializeField]
	public Camera davidSitting, OTS_TtoD, OTS_DtoT, twoShot, selectAnswer, topDown;

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
				currentTherapySession = allTherapySessions [GameManager.s_instance.day];
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
				SetCamera (OTS_DtoT);
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
				SetCamera (OTS_TtoD);
				PlayerController.s_instance.allowInput = true;
			}

			break;

		
		case TherapistState.DavidSelectAnswer:
			if (switchToAnxietyAnimation) {
				switchToAnxietyAnimation = false;
				thisTherapistState = TherapistState.AnxietyAnimation;
				PlayerController.s_instance.SwitchToAnxietyCam ();
				answerPanel.SetActive (false);
				PlayerController.s_instance.allowInput = false;

			}
			break;

		case TherapistState.AnxietyAnimation:
			if (switchToTherapistWaitingToRespond) {
				switchToTherapistWaitingToRespond = false;
				thisTherapistState = TherapistState.TherapistWaitingToRespond;
				SetCamera (twoShot);
			}
			break;


		case TherapistState.TherapistWaitingToRespond:
			if (GenericTimer.RunGenericTimer(waitingTime, ref waitingTimer)) {
				thisTherapistState = TherapistState.TherapistResponse;
				SetCamera (OTS_DtoT);
				therapistSubtitle.gameObject.SetActive (true);
				therapistSubtitle.text = currentTherapySession.therapySessionElements [questionIndex].responseString;
				currentAudioClip = Resources.Load(therapyAudioDirectory + currentTherapySession.therapySessionElements [questionIndex].responseAudioPath) as AudioClip;
			}


			break;
		case TherapistState.TherapistResponse:
			if (GenericTimer.RunGenericTimer (askingQuestionTime + currentAudioClip.length, ref askingQuestionTimer)) {
				SetCamera (mainViewOfMultipleChoice);
				therapistSubtitle.gameObject.SetActive (false);
			}
			break;

		case TherapistState.CheckForNextQuestion:
			if (GenericTimer.RunGenericTimer (waitingTime, ref waitingTimer)) {
				if (currentTherapySession.therapySessionElements.Count > questionIndex + 1) {
					questionIndex++;
					thisTherapistState = TherapistState.AskingQuestion;
				} else {
					thisTherapistState = TherapistState.EndSession;
				}
			}
			break;

		case TherapistState.EndSession:

			break;
		
		}


	}

	public override void SelectItem () {
		if (currentTherapySession.therapySessionElements [questionIndex].answerChoices [selection] == currentTherapySession.therapySessionElements [questionIndex].correctAnswer) {
			//correct
			//play corrent audio then play therapist response
			//play alleviate anxiety animation
			PlayerController.s_instance.AlleviateAnxiety();
		} else {
			//wrong
			//play wrong audio then play therapist repsonse
			//play gain anxiety animation
			PlayerController.s_instance.ReceiveAnxiety ();

		}
		switchToAnxietyAnimation = true;
		//then goto tolstoy response
	}

}
