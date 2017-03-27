using UnityEngine;
using System.Collections;

public class TestPlayerController : MonoBehaviour {

	BaseInput input;
	
	public float moveSpeed = 3.0f;
	public float sprintSpeedMod = 1.8f;
	public float turnSpeed = 0.6f;

	Animator anim;
    //	public Animator topAnimator;
    //	public Animator bottomAnimator;
    FootstepHandler footstepHandler;
	Rigidbody rigidbody;
	public Transform cameraObj;
    public Camera gameplayCamera;
	LineRenderer laserTarget;

    public static TestPlayerController s_instance;

	public enum InputLock { CameraOnly, Locked, Unlocked }
	public InputLock lockInput = InputLock.Unlocked;

    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start () {
		GetComponentInChildren<Camera>().transform.parent = null;
        footstepHandler = GetComponent<FootstepHandler>();
		input = GetComponent<BaseInput>();
		rigidbody = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		laserTarget = GetComponentInChildren<LineRenderer>();

        GetComponent<RootMotion.FinalIK.AimIK>().solver.target = GetComponent<RootMotion.FinalIK.LookAtIK>().solver.target = cameraObj.FindChild("LaserEnd");
		GetComponent<RootMotion.FinalIK.AimIK>().solver.transform = laserTarget.transform.parent.FindChild("FirePos");
	}
	
	void Update () {
		if(lockInput == InputLock.Locked)
			return;

		if(lockInput == InputLock.Unlocked)
			HandleMovement();

		HandleAiming();

		if(input.reload) {
			GetComponent<WeaponManager>().Reload();
		}
	}

	public void FixedUpdate() {
		float speedMod = input.sprint ? sprintSpeedMod : 1.0f;
		rigidbody.angularVelocity = Vector3.zero;

		if (input.moveDir.magnitude > 0.0f) {
			Vector3 targetVelocity = transform.forward * moveSpeed * speedMod;
			Vector3 velocity = rigidbody.velocity;
			Vector3 velocityChange = (targetVelocity - velocity);
//			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
//			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
//			velocityChange.y = 0;
			rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
//			rigidbody.MovePosition(transform.position + transform.forward * moveSpeed * speedMod * Time.deltaTime);


			Vector3 lookDir = Vector3.zero;
			if (Mathf.Abs(input.moveDir.z) > 0.0f)
				lookDir += (transform.position - cameraObj.transform.position).normalized * Mathf.Sign(input.moveDir.z);
			if (Mathf.Abs(input.moveDir.x) > 0.0f)
				lookDir += Vector3.Cross(transform.up, (transform.position - cameraObj.transform.position).normalized) * Mathf.Sign(input.moveDir.x);
			lookDir.Normalize();
			lookDir.y = 0.0f;

			if (lookDir != Vector3.zero)
				rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir), turnSpeed * Time.deltaTime));
		}
	}

	public void HandleMovement() {
//		float speedMod = input.sprint ? sprintSpeedMod : 1.0f;
//
//		if (input.moveDir.magnitude > 0.0f) {
//			rigidbody.MovePosition(transform.position + transform.forward * moveSpeed * speedMod * Time.deltaTime);
//
//			Vector3 lookDir = Vector3.zero;
//			if (Mathf.Abs(input.moveDir.z) > 0.0f)
//				lookDir += (transform.position - cameraObj.transform.position).normalized * Mathf.Sign(input.moveDir.z);
//			if (Mathf.Abs(input.moveDir.x) > 0.0f)
//				lookDir += Vector3.Cross(transform.up, (transform.position - cameraObj.transform.position).normalized) * Mathf.Sign(input.moveDir.x);
//			lookDir.Normalize();
//			lookDir.y = 0.0f;
//
//			if (lookDir != Vector3.zero)
//				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDir), turnSpeed * Time.deltaTime);
//		}

		//Wonky code because idle: 0, walk: 0.5, sprint: 1
		anim.SetFloat("Sprint", input.sprint ? Mathf.Lerp(anim.GetFloat("Sprint"), 1f, 5f*Time.deltaTime) :  Mathf.Lerp(anim.GetFloat("Sprint"), 0f, 5f*Time.deltaTime));
		anim.SetFloat("Movement", Mathf.Lerp(anim.GetFloat("Movement"), input.moveDir.normalized.magnitude/2f + anim.GetFloat("Sprint")/2f, 14f*Time.deltaTime));

		if (input.moveDir.magnitude > 0.0f) {
			footstepHandler.PlayFootStep(Mathf.Lerp(anim.GetFloat("Movement"), input.moveDir.normalized.magnitude/2f + anim.GetFloat("Sprint")/2f, 14f*Time.deltaTime)); //this is always 1, I need to figure out how to differentiate between running and walking and pass it to this param
		} else {
			footstepHandler.CallCeaseFootStep();
		}
	}

	public void HandleAiming() {
		anim.SetBool("Aim", input.aim);

		if(input.aim) {
			gameplayCamera.GetComponent<CameraFollow>().zoomedIn = true;
			Vector3 targetPos = transform.FindChild("CameraTarget").localPosition;
			targetPos.x = 0.5f;
			transform.FindChild("CameraTarget").localPosition = Vector3.Lerp(transform.FindChild("CameraTarget").localPosition, targetPos, Time.deltaTime*4f);

			Vector3 lookDir = Vector3.zero;
			lookDir += (transform.position - cameraObj.transform.position).normalized * Mathf.Sign(input.moveDir.z);
			lookDir += Vector3.Cross(transform.up, (transform.position - cameraObj.transform.position).normalized) * Mathf.Sign(input.moveDir.x);
			lookDir.Normalize();
			lookDir.y = 0.0f;
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation (lookDir), 15f*Time.deltaTime);

			//			RaycastHit hit = new RaycastHit(); 
			//			if(Physics.Raycast(gameplayCamera.ScreenPointToRay(Input.mousePosition), out hit)) {
			laserTarget.gameObject.SetActive(true);
			laserTarget.SetPositions( new Vector3[] {laserTarget.transform.position, laserTarget.transform.position + (laserTarget.transform.up * 50f)});
			//			if(Mathf.Abs(transform.eulerAngles.y - Quaternion.Lerp(transform.rotation, Quaternion.LookRotation (lookDir), 11f*Time.deltaTime).eulerAngles.y) < 3f) {
			//				GetComponent<RootMotion.FinalIK.LookAtIK>().enabled = false;
			//				GetComponent<RootMotion.FinalIK.AimIK>().enabled = true;
			//			}
			//			}

			if(lockInput == InputLock.Unlocked) { //Could be cleaner than checking for this here
				if(input.shoot) {
					if(GetComponent<WeaponManager>().CanFire()) {
						anim.SetTrigger("Fire");
						GetComponent<WeaponManager>().Fire();
						cameraObj.GetComponent<CameraFollow>().Recoil(GetComponent<WeaponManager>().CurrentWeapon.recoil);
					}
				}
				if(input.melee) {
					anim.SetTrigger("punch");
					GetComponent<WeaponManager>().Melee();

					GetComponent<RootMotion.FinalIK.LookAtIK>().enabled = false;
					GetComponent<RootMotion.FinalIK.AimIK>().enabled = false;
				}
			}

		} else {
			gameplayCamera.GetComponent<CameraFollow>().zoomedIn = false;
			Vector3 targetPos = transform.FindChild("CameraTarget").localPosition;
			targetPos.x = 0f;
			transform.FindChild("CameraTarget").localPosition = Vector3.Lerp(transform.FindChild("CameraTarget").localPosition, targetPos, Time.deltaTime*4f);


			//Do i care about this every frame?
			laserTarget.gameObject.SetActive(false);
			GetComponent<RootMotion.FinalIK.LookAtIK>().enabled = true;
			GetComponent<RootMotion.FinalIK.AimIK>().enabled = false;
		}
	}

	public void AimDone() {
		GetComponent<RootMotion.FinalIK.LookAtIK>().enabled = false;
		GetComponent<RootMotion.FinalIK.AimIK>().enabled = true;
	}

    public void EnterTherapy ()
    {
		TestUIManager.instance.SetState(TestUIManager.UIState.Cutscene);
//		lockInput = InputLock.CameraOnly;
        anim.SetTrigger("sit");
    }

    public void ExitTherapy()
    {
		TestUIManager.instance.SetState(TestUIManager.UIState.None);
//		lockInput = InputLock.Unlocked;
        anim.SetTrigger("stand");
    }
}
