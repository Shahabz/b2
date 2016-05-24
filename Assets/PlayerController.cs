using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Animator thisAnimator;
	const string walkingBool = "isWalking";
	float walkAxis, rotateAxis;
	float speedScalar = .04f, rotateScalar = 4f;

	public float hunger, anxiety;
	public int devLevel, money;

	public static PlayerController s_instance;

	void Awake() {
		if (s_instance == null) {
			s_instance = this;
		} else {
			Destroy (this);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		walkAxis = Input.GetAxis ("Vertical");
		rotateAxis = Input.GetAxis ("Horizontal");
		HandleMovement ();
		HandleRotation ();
		HandleAnimation ();
	}

	void HandleMovement() {
		transform.Translate (Vector3.forward * speedScalar * walkAxis);
	}

	void HandleRotation () {
		transform.Rotate (0, rotateAxis * rotateScalar,0);
	}

	void HandleAnimation () {
		if (walkAxis > 0) {
			thisAnimator.SetBool (walkingBool, true);
		} else {
			thisAnimator.SetBool (walkingBool, false);
		}
	}
}
