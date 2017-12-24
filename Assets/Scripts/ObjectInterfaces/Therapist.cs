 using UnityEngine;
using System.Collections;
using UnityEngine.UI;

enum TherapistState {Idle, Introduction, AskingQuestion, DavidSelectAnswer, AnsweringQuestion, TherapistWaitingToRespond, TherapistResponse, CheckForNextQuestion, EndSession, DoneForTheDay};

public class Therapist : MultipleChoice {

	//Show text only on questions and answers, small in between dialogue is audio only.

	//STATE MACHINE SWITCHES
	public bool switchToIdle,switchToIntroduction,switchToAskingQuestion, switchToDavidSelectAnswer, switchToAnsweringState, switchToTherapistWaitingToRespond;

	//STATE MACHINE TIMERS
	float introductionTimer, askingQuestionTimer, waitingTimer;
	float introductionTime = 1f, askingQuestionTime = 1f, waitingTime = 1;
	bool hasAskedQuestion;

	[SerializeField] BaseInput input;

    const string therapyAudioDirectory = "Audio/Therapy/";
	//it would be cool to fade into an abstract space when questions are being asked, or perhaps select 
	[SerializeField]
	public Camera davidSitting, OTS_TtoD, OTS_DtoT, selectAnswer, topDown;

	[SerializeField]
	GameObject answerPanel;

	[SerializeField] Text therapistSubtitle, therapistThoughtText, choiceA, choiceB, choiceC, choiceD;

	[SerializeField] Animator tolsoyAnimator;
	[SerializeField] Transform davidTransform;

	TherapistState thisTherapistState;
	//so therapy cant happen twice
	bool hasTriggered;

	public AudioSource[] incorrectBark, correctBark, welcomeToTherapyDavidLuna;

	AudioClip currentAudioClip;

	[SerializeField]
	TherapySession[] allTherapySessions;
	TherapySession currentTherapySession; 

	int questionIndex = 0;

	public static Therapist s_instance;

	public Cinemachine.CinemachineVirtualCamera cam1, cam2;

//    NPInputManager thisNPInputManager;

	// Use this for initialization


    private void Start()
    {
//        thisNPInputManager = TestPlayerController.s_instance.GetComponent<NPInputManager>();
        //input = GameObject.FindObjectOfType<BaseInput>();
		currentTherapySession = allTherapySessions [0];

    } 

    private void OnEnable()
    {
        //print(GameManager.s_instance);
        //GameObject.FindObjectOfType<GameManager>().OnNextDay += ResetTherapistForTheDay;
        //GameManager.s_instance.OnNextDay += ResetTherapistForTheDay;
    }

    private void OnDisable()
    {
        //GameObject.FindObjectOfType<GameManager>().OnNextDay -= ResetTherapistForTheDay;
    }

    void ResetTherapistForTheDay()
    {

    }



    void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && !hasTriggered) {
			hasTriggered = true;
			StartTherapistSession();
		}
	}

	public void StartTherapistSession () {
		if(tolsoyAnimator!=null) tolsoyAnimator.SetTrigger("sitdown");
       // CameraManager.s_instance.ToggleGameplayCamera(false);
      //  CameraManager.s_instance.SetMainViewOnScene(mainViewOfMultipleChoice);
      //  CameraManager.s_instance.MultipleChoiceCameraOn();
        TestPlayerController.s_instance.EnterTherapy();
		TestPlayerController.s_instance.SetPlayerMode (PlayerMode.Cutscene);
		if (davidTransform) {
			TestPlayerController.s_instance.transform.position = davidTransform.position;
			TestPlayerController.s_instance.transform.rotation = davidTransform.rotation;
		}
		switchToIntroduction = true;
	}

	public void EndTherapistSession () {
      //  CameraManager.s_instance.ToggleGameplayCamera(true);
        TestPlayerController.s_instance.ExitTherapy();
		//thisTherapistState = TherapistState.DoneForTheDay;
		tolsoyAnimator.SetTrigger ("standup");

	}

	void Update() {
		switch (thisTherapistState) {
		case TherapistState.Idle:
			//Welcome to therapy David

			/*if (switchToIntroduction) {
				switchToIntroduction = false;
				thisTherapistState = TherapistState.Introduction;
				welcomeToTherapyDavidLuna[GameManager.s_instance.day].Play ();
			}*/
			break;


		case TherapistState.Introduction:
			if (GenericTimer.RunGenericTimer (introductionTime + welcomeToTherapyDavidLuna[GameManager.s_instance.day].clip.length, ref introductionTimer)){
				thisTherapistState = TherapistState.AskingQuestion;
				ShowAskingQuestionState ();
			}

			break;
		case TherapistState.AskingQuestion:
			if (GenericTimer.RunGenericTimer (askingQuestionTime + currentAudioClip.length, ref askingQuestionTimer)) {
				therapistSubtitle.gameObject.SetActive (false);

				//CameraManager.s_instance.SetCamera (OTS_TtoD);
			}

			break;

		
		case TherapistState.DavidSelectAnswer:
			if (input.shoot || input.melee || input.interact) {
				SelectItem ();
			}
			if (NPInputManager.input.Down.WasPressed) {
				ArrowUp ();
			}
			if (NPInputManager.input.Up.WasPressed) {
				ArrowDown ();
			}
           
			break;
			//how is the switch made from anxiety animation to therapistwaiting
		case TherapistState.AnsweringQuestion:
			if (GenericTimer.RunGenericTimer(currentAudioClip.length, ref waitingTimer)) {
				thisTherapistState = TherapistState.TherapistWaitingToRespond;
                ShowResponseState();

                }
                break;


		case TherapistState.TherapistWaitingToRespond:
			if (GenericTimer.RunGenericTimer(currentAudioClip.length, ref waitingTimer)) {
				thisTherapistState = TherapistState.CheckForNextQuestion;
				therapistSubtitle.gameObject.SetActive (true);
				therapistSubtitle.text = currentTherapySession.therapySessionElements [questionIndex].responseString;
			}
			break;

		case TherapistState.TherapistResponse:
			if (GenericTimer.RunGenericTimer (askingQuestionTime, ref askingQuestionTimer)) {
				//SetCamera (mainViewOfMultipleChoice);
				therapistSubtitle.gameObject.SetActive (false);
				thisTherapistState = TherapistState.CheckForNextQuestion;
			}
			break;

		case TherapistState.CheckForNextQuestion:
			if (GenericTimer.RunGenericTimer (waitingTime, ref waitingTimer)) {
				if (currentTherapySession.therapySessionElements.Count > questionIndex + 1) {
					questionIndex++;
					thisTherapistState = TherapistState.AskingQuestion;
					ShowAskingQuestionState ();
				} else {
					thisTherapistState = TherapistState.EndSession;
				}
			}
			break;

		case TherapistState.EndSession:
			EndTherapistSession ();
            //alleviate anxiety after session is over
			break;
		
		}
	}

	public override void SelectItem () {
        currentAudioClip = Resources.Load(therapyAudioDirectory + currentTherapySession.therapySessionElements [questionIndex].answerPaths[selection]) as AudioClip;
        GetComponent<AudioSource>().clip = currentAudioClip;
        GetComponent<AudioSource>().Play();
        if (currentTherapySession.therapySessionElements [questionIndex].answerChoices [selection] == currentTherapySession.therapySessionElements [questionIndex].correctAnswer) {
			//correct
			//play corrent audio then play therapist response
			//play alleviate anxiety animation
		} else {
			//wrong
			//play wrong audio then play therapist repsonse
			//play gain anxiety animation
			TestPlayerController.s_instance.GetComponent<HealthHandler> ().TakeStress(1);

		}
		answerPanel.SetActive (false);
		questionIndex++;
		SwitchToDialogueState ();
		//then goto tolstoy response
	}


	void ShowAskingQuestionState(){
		therapistSubtitle.gameObject.SetActive (true);
        //therapistThoughtText.gameObject.SetActive(false);
		TextManager.s_instance.SetSubtitle(currentTherapySession.therapySessionElements [questionIndex].questionString);
		currentAudioClip = Resources.Load(therapyAudioDirectory + currentTherapySession.therapySessionElements [questionIndex].questionAudioPath) as AudioClip;
		GetComponent<AudioSource> ().clip = currentAudioClip;
		GetComponent<AudioSource> ().Play ();
        CameraManager.s_instance.SetCamera (OTS_DtoT);
	}

	public void SwitchToAnswerState() {
		answerPanel.SetActive (true);
		cam1.enabled = false;
		cam2.enabled = true;
		choiceA.text = currentTherapySession.therapySessionElements [questionIndex].answerChoices [0];
		choiceB.text = currentTherapySession.therapySessionElements [questionIndex].answerChoices [1];
		//			choiceC.text = currentTherapySession.therapySessionElements [questionIndex].answerChoices [2];
		//		choiceD.text = currentTherapySession.therapySessionElements [questionIndex].answerChoices [3];
		StartCoroutine(ShittyHackaround());
	}

	IEnumerator ShittyHackaround() {
		yield return new WaitForSeconds (.03f);
		thisTherapistState = TherapistState.DavidSelectAnswer;

	}

	public void SwitchToDialogueState() {
		//			choiceC.text = currentTherapySession.therapySessionElements [questionIndex].answerChoices [2];
		//		choiceD.text = currentTherapySession.therapySessionElements [questionIndex].answerChoices [3];
		thisTherapistState = TherapistState.Idle;
		if (GetComponent<DialogueSystem> () != null) {
			GetComponent<DialogueSystem> ().SetActiveState (true);
			GetComponent<DialogueSystem> ().PlayNext ();
		}
		cam1.enabled = true;
		cam2.enabled = false;
	}

    void ShowResponseState()
    {
        //therapistThoughtText.gameObject.SetActive(true);
        //therapistThoughtText.text = currentTherapySession.therapySessionElements[questionIndex].responseString;
        currentAudioClip = Resources.Load(therapyAudioDirectory + currentTherapySession.therapySessionElements[questionIndex].responsePath) as AudioClip;
        GetComponent<AudioSource>().clip = currentAudioClip;
        GetComponent<AudioSource>().Play();
       // CameraManager.s_instance.SetCamera(mainViewOfMultipleChoice);
    }
}
