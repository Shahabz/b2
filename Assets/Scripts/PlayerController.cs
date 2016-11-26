using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

using InControl;

public enum PlayerState {Walking, Computer, PracticeProgramming, Therapy, AppliedToJob};
public enum AnxietyDescription {None, Mild, Moderate, Severe, Debilitating, Psychotic, _Size}
public enum DevLevel {None, Amateur=10, Inexperienced = 20, Beginner=30, Novice=50, Junior=70, Green = 90, Intermediate = 100, _Size}

public class PlayerController : MonoBehaviour {
	public static PlayerController s_instance;

	PlayerState thisPlayerState = PlayerState.Walking;
	public Animator thisAnimator;
	const string walkingBool = "fwd", walkingBackBool = "back";
	float walkAxis, rotateAxis;
	public float speedScalar = .04f, rotateScalar = 4f;
    public float moneyLeft;

	public int anxietyLevel, lastAnxietyLevel;
	public int devLevel, lastDevLevel;

	Color brainFlashColor, brainNormalColor = new Color (1f,1f,1f, .5f), brainRedColor = new Color (1f,0,0,.5f), brainGreenColor = new Color(0,1f,0,.5f);

	[SerializeField]
	Camera anxietyCam, jobCam;
	public Computer currentComputer;

	const string AnxietyTextTemplate = "Anxiety Level: ";
    const string DevLevelTextTemplate = "Dev Level: ";

    public AnimatedSlider anxietySlider, devSlider;
    public Text moneyLeftText, changeInMoneyText;

	InputDevice inputDevice;

	public bool isNearComputer = false, isNearTherapist = false;

	[SerializeField]
	GameObject brain;
	Vector3 brainStartPosition, anxietyCamStartPosition;
	float brainFloatUpDistance = .19f, brainRaiseSpeed = .3f, brainFlashSpeed = .44f, brainFlashTimer, brainFlashDuration = 3f, brainFlashDurationTimer;
	bool brainFlashState;
	bool isRaisingBrain, isFlashingBrain;
	public bool allowSelectionInput;

    //computer bools and timers
    public bool isPsychoProgramming, isPracticingProgramming, isApplyingToJob;
    float programmingPracticeTime = 3.5f, programmingPracticeTimer, jobResponseTime = 3.5f, jobResponseTimer, showJobResponseTime, showJobResponseTimer;


    void Awake() {
		if (s_instance == null) {
			s_instance = this;
		} else {
			Destroy (this);
		}
		brainStartPosition = brain.transform.localPosition;
		anxietyCamStartPosition = anxietyCam.transform.localPosition;
	}
		

	#region StateMachine

	public bool switchToPracticeProgramming, switchToComputer, switchToWalking, switchToApplyToJob, switchToTherapy;

	void Update () {
        if (Input.GetKeyDown(KeyCode.F)) {
            thisAnimator.SetTrigger("punch");
        }
		inputDevice = InputManager.ActiveDevice;

		switch (thisPlayerState) {
		case PlayerState.Walking:
			if (switchToComputer) {
				switchToComputer = false;
                thisPlayerState = PlayerState.Computer;
				thisAnimator.SetBool (walkingBackBool, false);
				thisAnimator.SetBool (walkingBool, false);
				return;
			}
			HandleLeftStickVertical ();
			HandleLeftStickHorizontal ();
			HandleMovement ();
			HandleRotation ();
			HandleAnimation ();
			if (inputDevice.Action1.WasPressed) {
				Interact ();
			}

			if (switchToTherapy) {
				switchToTherapy = false;
				thisPlayerState = PlayerState.Therapy;
				thisAnimator.SetTrigger ("sit");
				Therapist.s_instance.StartTherapistSession ();
			}

			break;
		case PlayerState.Computer:
			if (inputDevice.LeftStickUp.WasPressed) {
				currentComputer.ArrowUp ();
			} else if (inputDevice.LeftStickDown.WasPressed) {
				currentComputer.ArrowDown ();
			}
			if (inputDevice.Action1.WasPressed) {
				currentComputer.SelectItem ();
			}
			if (switchToWalking) {
				switchToWalking = false;
				thisPlayerState = PlayerState.Walking;
			}
			if (switchToPracticeProgramming) {
				switchToPracticeProgramming = false;
				SwitchToAnxietyCam ();
				thisPlayerState = PlayerState.PracticeProgramming;
			}
				
			if (switchToApplyToJob) {
				Camera.main.transform.rotation = jobCam.transform.rotation;
				Camera.main.transform.position = jobCam.transform.position;
                showJobResponseTime = JobText.s_instance.GetJobDescriptionScrollTime();
                StartCoroutine("WaitToShowJob");
                switchToApplyToJob = false;
				thisPlayerState = PlayerState.AppliedToJob;
			}
			break;

		case PlayerState.PracticeProgramming:
                if (isPracticingProgramming){
                    ShowProgrammingPractice();
                }
                if (!isPsychoProgramming)
                {
                    HandleBrainAnimation();
                    if (switchToComputer)
                    {
                        switchToComputer = false;
                        thisPlayerState = PlayerState.Computer;
                        currentComputer.MultipleChoiceCameraOn();
                    }
                    if (switchToWalking)
                    {
                        switchToWalking = false;
                        thisPlayerState = PlayerState.Walking;
                    }
                }
                
			break;

		case PlayerState.AppliedToJob:
                if (isApplyingToJob)
                {
                    ShowJobResponse();
                }
			if (switchToComputer) {
				switchToComputer = false;
				thisPlayerState = PlayerState.Computer;
				currentComputer.MultipleChoiceCameraOn ();
			}
			break;

		case PlayerState.Therapy:
			if (inputDevice.LeftStickUp.WasPressed && allowSelectionInput) {
				Therapist.s_instance.ArrowUp ();
			} else if (inputDevice.LeftStickDown.WasPressed && allowSelectionInput) {
				Therapist.s_instance.ArrowDown ();
			}
			if (inputDevice.Action1.WasPressed && allowSelectionInput) {
				Therapist.s_instance.SelectItem ();
			}
			HandleBrainAnimation ();

			if (switchToWalking) {
				switchToWalking = false;
				thisAnimator.SetTrigger ("stand");
				thisPlayerState = PlayerState.Walking;
			}
			break;

		}
	}
	#endregion

	#region Movement+Animation
	void HandleMovement() {
		transform.Translate (Vector3.forward * speedScalar * walkAxis);
	}

	void HandleRotation () {
		transform.Rotate (0, rotateAxis * rotateScalar,0);
	}

	void Interact() {
		if (isNearComputer) {
			switchToComputer = true;
			currentComputer.SetLastInLevelCamTransform();
			currentComputer.TurnOn ();
		}

		if (isNearTherapist) {
			switchToTherapy = true;
		}
	}

	void HandleAnimation () {
		if (walkAxis > 0) {
			thisAnimator.SetBool (walkingBool, true);
			thisAnimator.SetBool (walkingBackBool, false);
		} else if (walkAxis < 0) {
			thisAnimator.SetBool (walkingBackBool, true);
			thisAnimator.SetBool (walkingBool, false);
		} else {
			thisAnimator.SetBool (walkingBackBool, false);
			thisAnimator.SetBool (walkingBool, false);
		}
	}
	void HandleLeftStickVertical () {
		walkAxis = 0;
		if (inputDevice.LeftStickUp != 0) {
			walkAxis = inputDevice.LeftStickUp;
		} else if (inputDevice.LeftStickDown != 0) {
			walkAxis = -inputDevice.LeftStickDown;
		}

	}

	void HandleLeftStickHorizontal () {
		// Strafe
		rotateAxis = 0;
		if (inputDevice.LeftStickLeft != 0) {
			rotateAxis = -inputDevice.LeftStickLeft;
		} else if (inputDevice.LeftStickRight != 0) {
			rotateAxis = inputDevice.LeftStickRight;
		}
	}
	#endregion
	#region BrainAnimation


	void HandleBrainAnimation() {
		if (isRaisingBrain) {
			RaiseBrain ();
		}
		else if (isFlashingBrain) {
			FlashBrain ();
			anxietySlider.AnimateSlider(anxietyLevel, lastAnxietyLevel, AnxietyTextTemplate, ((AnxietyDescription)anxietyLevel).ToString());
		}
	}

	void RaiseBrain () {
		if (Vector3.Distance (brainStartPosition, brain.transform.localPosition) < brainFloatUpDistance) {
			brain.transform.Translate (Vector3.up * Time.deltaTime * brainRaiseSpeed);
			Camera.main.transform.Translate (Vector3.up * Time.deltaTime * brainRaiseSpeed);
		} else {
			DisplayAnxiety (true);
			isRaisingBrain = false;
		}
	}
	void FlashBrain () {
		brainFlashDurationTimer += Time.deltaTime;
		brainFlashTimer += Time.deltaTime;
		if (brainFlashTimer > brainFlashSpeed) {
			AlternateBrainColor ();
			brainFlashTimer = 0;
		}
		if (brainFlashDurationTimer > brainFlashDuration) {
			HandleAnxietyCamTransition ();
		}
	}

	void HandleAnxietyCamTransition() {
		isFlashingBrain = false;
		brainFlashDurationTimer = 0;
		ResetAnxietyCamAndBrainPosition ();
		switch (thisPlayerState) {
		case PlayerState.PracticeProgramming:
			switchToComputer = true;
			break;
		case PlayerState.Walking :
			switchToWalking = true;
			break;
		case PlayerState.Therapy:
			Therapist.s_instance.switchToTherapistWaitingToRespond = true;
			break;
		}
	}	
	
	void AlternateBrainColor () {
		if (brainFlashState) {
			brain.GetComponentInChildren<MeshRenderer> ().material.color = brainNormalColor;
		} else {
			brain.GetComponentInChildren<MeshRenderer> ().material.color = brainFlashColor;
		}
		brainFlashState = !brainFlashState;
	}
	public void ResetAnxietyCamAndBrainPosition() {
		anxietyCam.transform.localPosition = anxietyCamStartPosition;
		brain.transform.localPosition = brainStartPosition;
		brainFlashState = false;
		brain.SetActive (false);
		DisplayAnxiety (false);

	}
	#endregion

	#region AnxietyDisplay
	void DisplayAnxiety (bool on) {
		anxietySlider.gameObject.SetActive (on);
		anxietySlider.thisText.text = AnxietyTextTemplate + ((AnxietyDescription)lastAnxietyLevel).ToString();
	}

	public void ReceiveAnxiety() {
        GetComponentInChildren<CodeThoughts>().StopSpawning();
        lastAnxietyLevel = anxietyLevel;
        anxietyLevel++;
        if (isPsychoProgramming)
        {
            //throw punch and rage quit
            StartCoroutine("RageQuit");
        }
        else
        {
            brain.SetActive(true);
            isRaisingBrain = true;
            isFlashingBrain = true;
            brainFlashColor = brainRedColor;
        }
	}

    IEnumerator RageQuit()
    {
        thisAnimator.SetTrigger("punch");
        currentComputer.PunchComputer();
        yield return new WaitForSeconds(2f);
        thisPlayerState = PlayerState.Walking;

    }

	public void AlleviateAnxiety() {
		isRaisingBrain = true;
		isFlashingBrain = true;
		lastAnxietyLevel = anxietyLevel;
		anxietyLevel--;
		brainFlashColor = brainGreenColor;
	}

	public void SwitchToAnxietyCam() {
		Camera.main.transform.rotation = anxietyCam.transform.rotation;
		Camera.main.transform.position = anxietyCam.transform.position;
	}

    #endregion
    #region ComputerProgrammingLogic
    public void TryPracticeProgramming()
    {
        print(anxietyLevel);

        //need to make a version of programming practice that is psychotic which leads to punch
        if (anxietyLevel <= 1)
        {
            StartProgramming(false);
        }
        else
        {
            float chanceOfRage = UnityEngine.Random.Range(0f, 10f);
            chanceOfRage = chanceOfRage * anxietyLevel;
            if (chanceOfRage < 7)
            {
                StartProgramming(false);
            }
            else
            {
                StartProgramming(true);
            }
        }
    }

    void StartProgramming(bool isPsycho)
    {
        GetComponentInChildren<CodeThoughts>().StartSpawning(isPsycho);
        switchToPracticeProgramming = true;
        isPracticingProgramming = true;
        isPsychoProgramming = isPsycho;
        lastDevLevel = devLevel;
        devLevel++;
        devSlider.thisText.text = DevLevelTextTemplate + GetDevLevelString();
        devSlider.gameObject.SetActive(true);
    }

    string GetDevLevelString() {
        string outString;
        foreach (DevLevel x in Enum.GetValues(typeof(DevLevel)))
        {
            if (devLevel <= (int)x)
            {
                outString = x.ToString();
                return outString;
            }
        }

        outString = "ERROR";
        return outString;
    }


    void ShowProgrammingPractice()
    {
        //visual representation of coding
        if (programmingPracticeTimer < programmingPracticeTime)
        {
            devSlider.thisSlider.value = Mathf.Lerp(lastDevLevel, devLevel, programmingPracticeTimer / programmingPracticeTime);
            programmingPracticeTimer += Time.deltaTime;
            return;
        }
        else
        {
            devSlider.gameObject.SetActive(false);
            isPracticingProgramming = false;
            programmingPracticeTimer = 0;
            PlayerController.s_instance.ReceiveAnxiety();
        }
    }

    IEnumerator WaitToShowJob()
    {
        yield return new WaitForSeconds(3f);
        isApplyingToJob = true;
        JobText.s_instance.SpawnJobText();
    }

    void ShowJobResponse()
    {
        //wait for response

        //show response
        if (showJobResponseTimer < showJobResponseTime)
        {
            showJobResponseTimer += Time.deltaTime;
        }

        else
        {
            programmingPracticeTimer = 0;
            isApplyingToJob = false;
            currentComputer.appliedOnThisComputerToday = true;
            switchToComputer = true;
            currentComputer.UpdateJobState();
        }
    }
    #endregion
}
