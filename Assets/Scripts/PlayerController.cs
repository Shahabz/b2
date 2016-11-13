using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using InControl;

public enum PlayerState {Walking, Computer, PracticeProgramming, Therapy, AppliedToJob};
public enum AnxietyDescription {None, Mild, Moderate, Severe, Debilitating, Psychotic, _Size}

public class PlayerController : MonoBehaviour {
	public static PlayerController s_instance;

	PlayerState thisPlayerState = PlayerState.Walking;
	public Animator thisAnimator;
	const string walkingBool = "fwd", walkingBackBool = "back";
	float walkAxis, rotateAxis;
	public float speedScalar = .04f, rotateScalar = 4f;

	int anxietyLevel, lastAnxietyLevel;
	public int devLevel;

	Color brainFlashColor, brainNormalColor = new Color (1f,1f,1f, .5f), brainRedColor = new Color (1f,0,0,.5f), brainGreenColor = new Color(0,1f,0,.5f);

	[SerializeField]
	Camera anxietyCam, jobCam;
	public Computer currentComputer;

	const string AnxietyTextTemplate = "Anxiety Level: ";

	InputDevice inputDevice;

	public bool isNearComputer = false, isNearTherapist = false;

	
	public Slider anxietySlider;
	float anxietSliderLerp;
	public Text anxietyText;
	float sliderSpeed = .9f;
	
	[SerializeField]
	GameObject brain;
	Vector3 brainStartPosition, anxietyCamStartPosition;
	float brainFloatUpDistance = .19f, brainRaiseSpeed = .3f, brainFlashSpeed = .44f, brainFlashTimer, brainFlashDuration = 3f, brainFlashDurationTimer;
	bool brainFlashState;
	bool isRaisingBrain, isFlashingBrain;
	public bool allowSelectionInput;


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
		inputDevice = InputManager.ActiveDevice;

		switch (thisPlayerState) {
		case PlayerState.Walking:
			if (switchToComputer) {
				switchToComputer = false;
				thisPlayerState = PlayerState.Computer;
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
				brain.SetActive (true);
			}
				
			if (switchToApplyToJob) {
				Camera.main.transform.rotation = jobCam.transform.rotation;
				Camera.main.transform.position = jobCam.transform.position;
				switchToApplyToJob = false;
				thisPlayerState = PlayerState.AppliedToJob;
			}
			break;

		case PlayerState.PracticeProgramming:
			HandleBrainAnimation ();
			if (switchToComputer) {
				switchToComputer = false;
				thisPlayerState = PlayerState.Computer;
				currentComputer.MultipleChoiceCameraOn ();
			}
			if (switchToWalking) {
				switchToWalking = false;
				thisPlayerState = PlayerState.Walking;
			}
			break;

		case PlayerState.AppliedToJob:
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
			currentComputer.TurnOn ();
			currentComputer.MultipleChoiceCameraOn ();
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
		//Car Acceleration
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

	void RaiseBrain () {
		if (Vector3.Distance (brainStartPosition, brain.transform.localPosition) < brainFloatUpDistance) {
			brain.transform.Translate (Vector3.up * Time.deltaTime * brainRaiseSpeed);
			Camera.main.transform.Translate (Vector3.up * Time.deltaTime * brainRaiseSpeed);
		} else {
			DisplayAnxiety (true);
			isRaisingBrain = false;
		}
	}

	void HandleBrainAnimation() {
		if (isRaisingBrain) {
			RaiseBrain ();
		}
		else if (isFlashingBrain) {
			FlashBrain ();
			AnimateAnxietyBar ();
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
		anxietyText.gameObject.SetActive (on);
		anxietyText.text = AnxietyTextTemplate + ((AnxietyDescription)anxietyLevel-1).ToString();
	}

	void AnimateAnxietyBar() {
		if (anxietySlider.value != anxietyLevel) {
			anxietSliderLerp += Time.deltaTime * sliderSpeed;
			anxietySlider.value = Mathf.Lerp (lastAnxietyLevel, anxietyLevel, anxietSliderLerp);
		} else {
			anxietyText.text = AnxietyTextTemplate + ((AnxietyDescription)anxietyLevel).ToString();
			anxietSliderLerp = 0;
		}
	}

	public void ReceiveAnxiety() {
		isRaisingBrain = true;
		isFlashingBrain = true;
		lastAnxietyLevel = anxietyLevel;
		anxietyLevel++;
		brainFlashColor = brainRedColor;
	}

	public void AlleviateAnxiety() {
		isRaisingBrain = true;
		isFlashingBrain = true;
		lastAnxietyLevel = anxietyLevel;
		anxietyLevel--;
		brainFlashColor = brainGreenColor;
	}

	public void SwitchToAnxietyCam() {
		brain.SetActive (true);
		Camera.main.transform.rotation = anxietyCam.transform.rotation;
		Camera.main.transform.position = anxietyCam.transform.position;
	}

	#endregion
		
}
