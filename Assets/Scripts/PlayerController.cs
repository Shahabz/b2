using UnityEngine;
using System.Collections;
using InControl;

public enum PlayerState{Walking, Computer, AnxietyCam, Therapy};

public class PlayerController : MonoBehaviour {
	public static PlayerController s_instance;

	PlayerState thisPlayerState = PlayerState.Walking;
	public Animator thisAnimator;
	const string walkingBool = "isWalking";
	float walkAxis, rotateAxis;
	public float speedScalar = .04f, rotateScalar = 4f;

	public int anxiety;
	public int devLevel;

	Color brainFlashColor;

	[SerializeField]
	Camera anxietyCam;
	public Computer currentComputer;


	InputDevice inputDevice;

	public bool isNearComputer = false;

	[SerializeField]
	GameObject brain;
	Vector3 brainStartPosition, anxietyCamStartPosition;
	float brainFloatUpDistance = .28f, brainRaiseSpeed = .3f, brainFlashSpeed = .1f, brainFlashTimer, brainFlashDuration;
	bool brainFlashState;
	bool isBrainRaisingAndFlashing;


	void Awake() {
		if (s_instance == null) {
			s_instance = this;
		} else {
			Destroy (this);
		}
		brainStartPosition = brain.transform.localPosition;
		anxietyCamStartPosition = anxietyCam.transform.localPosition;
	}

	// Use this for initialization
	void Start () {
	
	}

	public void ReceiveAnxiety() {
		isBrainRaisingAndFlashing = true;
		anxiety++;
		brainFlashColor = Color.red;
	}

	public void AlleviateAnxiety() {
		isBrainRaisingAndFlashing = true;
		anxiety--;
		brainFlashColor = Color.green;
	}

	#region StateMachine

	public bool switchToAnxietyCam, switchToComputer, switchToWalking;


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
			if (switchToAnxietyCam) {
				switchToAnxietyCam = false;
				Camera.main.transform.rotation = anxietyCam.transform.rotation;
				Camera.main.transform.position = anxietyCam.transform.position;
				thisPlayerState = PlayerState.AnxietyCam;
				brain.SetActive (true);
			}
			break;

		case PlayerState.AnxietyCam:
			if (isBrainRaisingAndFlashing) {
				RaiseAndFlashBrain ();
			}
			if (switchToComputer) {
				switchToComputer = false;
				thisPlayerState = PlayerState.Computer;
			}
			break;
		}
	}
	#endregion
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
			currentComputer.ComputerCameraOn ();
		}
	}

	void HandleAnimation () {
		if (walkAxis > 0) {
			thisAnimator.SetBool (walkingBool, true);
		} else {
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

	void RaiseAndFlashBrain () {
		if (Vector3.Distance (brainStartPosition, brain.transform.localPosition) < brainFloatUpDistance) {
			brain.transform.Translate (Vector3.up* Time.deltaTime * brainRaiseSpeed);
			Camera.main.transform.Translate (Vector3.up* Time.deltaTime * brainRaiseSpeed);

		} else if (!brainFlashState) {
			brainFlashState = true;
		}



		brain.SetActive (false);
		switchToComputer = true;
		currentComputer.ComputerCameraOn ();
		ResetAnxietyCamAndBrainPosition ();
	}

	void FlashBrain () {
		brainFlashDuration += Time.deltaTime;
		brainFlashTimer += Time.deltaTime;
		if (brainFlashTimer > brainFlashSpeed) {
			if (brainFlashState) {
				brain.GetComponentInChildren<MeshRenderer> ().material.color = Color.white;
			} else {
				brain.GetComponentInChildren<MeshRenderer> ().material.color = brainFlashColor;
			}
		}
	}
	void ResetAnxietyCamAndBrainPosition() {
		anxietyCam.transform.localPosition = anxietyCamStartPosition;
		brain.transform.localPosition = brainStartPosition;
		isBrainRaisingAndFlashing = false;
		brainFlashState = false;
		brain.SetActive (false);


	}
		
}
