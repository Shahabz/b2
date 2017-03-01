﻿using UnityEngine;
using System.Collections;

public class TestPlayerController : MonoBehaviour {

	BaseInput input;
	
	public float moveSpeed = 3.0f;
	public float sprintSpeedMod = 1.8f;
	public float turnSpeed = 0.6f;

	Animator anim;
//	public Animator topAnimator;
//	public Animator bottomAnimator;

	Rigidbody rigidbody;
	
	public Transform cameraObj;
	LineRenderer laserTarget;
	
	void Start () {
		input = GetComponent<BaseInput>();
		rigidbody = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		laserTarget = GetComponentInChildren<LineRenderer>();
	}
	
	void Update () {
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

		//Wonky code because idle: 0, walk: 0.5, sprint: 1
		anim.SetFloat("Sprint", input.sprint ? Mathf.Lerp(anim.GetFloat("Sprint"), 1f, 5f*Time.deltaTime) :  Mathf.Lerp(anim.GetFloat("Sprint"), 0f, 5f*Time.deltaTime));
		anim.SetFloat("Movement", Mathf.Lerp(anim.GetFloat("Movement"), input.moveDir.normalized.magnitude/2f + anim.GetFloat("Sprint")/2f, 14f*Time.deltaTime));
		anim.SetBool("Aim", input.aim);

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
			cameraObj.GetComponent<CameraFollow>().Recoil(0.5f);
		}
		if(input.melee) {
			anim.SetTrigger("punch");
			GetComponent<WeaponManager>().Melee();
		}
	}

	public void AimDone() {
		GetComponent<RootMotion.FinalIK.LookAtIK>().enabled = false;
		GetComponent<RootMotion.FinalIK.AimIK>().enabled = true;
	}
}
