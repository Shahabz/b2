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
	
//	public LayerMask mask;
	
//	public float jumpStrength = 3.0f;
	
//	float snowLevel = 0.0f;
	
//	public float snowAccumSpeed = 0.2f;
	
//	bool moved;

	Rigidbody rigidbody;
	
//	[System.NonSerialized]					
//	public float lookWeight;					// the amount to transition when using head look	
//	
//	public float lookSmoother = 3f;				// a smoothing setting for camera motion
	
	public Transform cameraObj;

//	public Transform hand;

//	public float attackRadius = 1.0f;

	LineRenderer laserTarget;
	
	void Start () {
		input = GetComponent<BaseInput>();
		rigidbody = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		laserTarget = GetComponentInChildren<LineRenderer>();
	}
	
	void Update () {
//		if(Input.GetKeyDown(KeyCode.T)) {
//			Application.LoadLevel(0);
//		}
//		if(Input.GetKeyDown(KeyCode.Escape)) {
//			Application.Quit();
//		}
//		//-------------------Snow stuff---------------------//
//		if (moved) {
//			snowLevel -= snowAccumSpeed * 5.0f * Time.deltaTime;
//			if(snowLevel < 0.1f) {
//				snowLevel = 0.1f;
//				moved = false;
//			}
//		} else {
//			snowLevel += snowAccumSpeed * Time.deltaTime;
//			if (snowLevel > 1.0f)
//				snowLevel = 1.0f;
//			if (rigidbody.velocity.sqrMagnitude > 0.05f || input.dir.magnitude > 0.0f) 
//				moved = true;
//		}
//		mat.SetFloat ("_Snow", snowLevel);
//		
//		bottomAnimator.SetFloat ("SnowLevel", snowLevel);
//		topAnimator.SetFloat ("SnowLevel", snowLevel);
		
		//---------------------------------------------------//
		//---------------------Movement----------------------//
		float speedMod = input.sprint ? sprintSpeedMod : 1.0f;
		rigidbody.angularVelocity = Vector3.zero;
		
		if (input.moveDir.magnitude > 0.0f) {
			rigidbody.MovePosition(transform.position + transform.forward * moveSpeed * speedMod * Time.deltaTime);
			
			Vector3 lookDir = Vector3.zero;
			if(Mathf.Abs(input.moveDir.z) > 0.0f)
				lookDir += (transform.position - cameraObj.transform.position).normalized * Mathf.Sign(input.moveDir.z);
			if(Mathf.Abs(input.moveDir.x) > 0.0f)
				lookDir += Vector3.Cross(transform.up, (transform.position - cameraObj.transform.position).normalized) * Mathf.Sign(input.moveDir.x);
			lookDir.Normalize();
			lookDir.y = 0.0f;

			if(lookDir != Vector3.zero)
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation (lookDir), turnSpeed*Time.deltaTime);
		}


		//TODO fix this
		anim.SetFloat("Sprint", input.sprint ? Mathf.Lerp(anim.GetFloat("Sprint"), 1f, 0.06f) :  Mathf.Lerp(anim.GetFloat("Sprint"), 0f, 0.06f));
		anim.SetFloat("Movement", input.moveDir.magnitude/2f + anim.GetFloat("Sprint")/2f);
		anim.SetBool("Aim", input.aim);

		transform.FindChild("Trail").gameObject.SetActive(input.sprint);

		if(input.aim) {
			Camera.main.GetComponent<CameraFollow>().distanceMax = Mathf.Lerp(Camera.main.GetComponent<CameraFollow>().distanceMax, 1f, Time.deltaTime*5f);
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
//			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
			laserTarget.gameObject.SetActive(true);
			laserTarget.SetPositions( new Vector3[] {laserTarget.transform.position, laserTarget.transform.position + (laserTarget.transform.up * 50f)});
//			if(Mathf.Abs(transform.eulerAngles.y - Quaternion.Lerp(transform.rotation, Quaternion.LookRotation (lookDir), 11f*Time.deltaTime).eulerAngles.y) < 3f) {
//				GetComponent<RootMotion.FinalIK.LookAtIK>().enabled = false;
//				GetComponent<RootMotion.FinalIK.AimIK>().enabled = true;
//			}
//			}

		} else {
			Camera.main.GetComponent<CameraFollow>().distanceMax = Mathf.Lerp(Camera.main.GetComponent<CameraFollow>().distanceMax, 1.8f, Time.deltaTime*5f);
			Vector3 targetPos = transform.FindChild("CameraTarget").localPosition;
			targetPos.x = 0f;
			transform.FindChild("CameraTarget").localPosition = Vector3.Lerp(transform.FindChild("CameraTarget").localPosition, targetPos, Time.deltaTime*4f);

			laserTarget.gameObject.SetActive(false);
			GetComponent<RootMotion.FinalIK.LookAtIK>().enabled = true;
			GetComponent<RootMotion.FinalIK.AimIK>().enabled = false;
		}

		if(input.shoot) {
			anim.SetTrigger("Fire");
			GetComponent<WeaponManager>().Fire();
		}
		if(input.melee) {
			anim.SetTrigger("punch");
			GetComponent<WeaponManager>().Melee();
		}

//		if(Input.GetButton("Fire2"))
//		{
//			// ...set a position to look at with the head, and use Lerp to smooth the look weight from animation to IK (see line 54)
//			topAnimator.SetLookAtPosition(enemy.transform.position);
//			lookWeight = Mathf.Lerp(lookWeight,1f,Time.deltaTime*lookSmoother);
//			
//		}
//		// else, return to using animation for the head by lerping back to 0 for look at weight
//		else
//		{
//			lookWeight = Mathf.Lerp(lookWeight,0f,Time.deltaTime*lookSmoother);
//		}
//		topAnimator.SetLookAtWeight(lookWeight);
	}

	public void AimDone() {
		GetComponent<RootMotion.FinalIK.LookAtIK>().enabled = false;
		GetComponent<RootMotion.FinalIK.AimIK>().enabled = true;
	}
	
//	bool CheckGrounded() {
//		Debug.DrawRay (transform.position + (Vector3.up * 0.1f), Vector3.down, Color.red, 1.0f);
//		return Physics.Raycast (transform.position + Vector3.up, Vector3.down, GROUND_CHECK_DISTANCE, mask);
//	}
	
//	void Attack() {
//		Collider[] cols = Physics.OverlapSphere(hand.transform.position, attackRadius);
//		foreach(Collider col in cols) {
//			col.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
//		}
//	}
}
