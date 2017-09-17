using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XanaxGirl : GirlController {

	public XanaxWaypoint[] xanaxWaypoints;
	public Transform holdObject;
	XanaxWaypoint currentXanWaypoint;
	enum XanaxGirlState {normal, lookingforxanax, walkingtoxanax, taking_xanax, gotocomputer, turnoncomputer, dancing, dying};
	XanaxGirlState thisState = XanaxGirlState.normal;
	bool SwitchToComputerState;
	public Transform computerWaypoint, laptop;	

	float timerforturningoncomputer;

	public void SwitchToXanaxSearch () {
		thisState = XanaxGirlState.lookingforxanax;
	}

	void Start () {
		xanaxWaypoints = FindObjectsOfType<XanaxWaypoint> ();
	}

	// Update is called once per frame
	void Update () {
		switch (thisState) {
		case XanaxGirlState.normal:
			base.Update ();
			break;

		case XanaxGirlState.lookingforxanax:
			foreach (XanaxWaypoint x in xanaxWaypoints) {
				if (x.hasBeenAccessed == false) {
					currentXanWaypoint = x;
					thisState = XanaxGirlState.walkingtoxanax;
					return;
				}
			}
			thisState = XanaxGirlState.gotocomputer;
			break;

		case XanaxGirlState.walkingtoxanax:
			thisNavMeshAgent.SetDestination (currentXanWaypoint.transform.position);
			if (Vector3.Distance (currentXanWaypoint.transform.position, transform.position) < .1) {
				if (currentXanWaypoint.hasBeenAccessed == false) {
					thisState = XanaxGirlState.taking_xanax;
					StartCoroutine ("switchbacktolookingforxanax");
					currentXanWaypoint.thisXanaxPickup.transform.SetParent (holdObject);
					currentXanWaypoint.thisXanaxPickup.transform.localPosition = Vector3.zero;
					currentXanWaypoint.thisXanaxPickup.transform.localRotation = Quaternion.identity;
					GetComponent<Animator> ().SetTrigger ("pill");
					currentXanWaypoint.hasBeenAccessed = true;
				}
			}
			break;

		case XanaxGirlState.taking_xanax:
				
			break;

		case XanaxGirlState.gotocomputer:
			//walk tocomputer
			thisNavMeshAgent.SetDestination (computerWaypoint.transform.position);
			if (Vector3.Distance (computerWaypoint.transform.position, transform.position) < .1) {
			//check if reached
				thisState = XanaxGirlState.turnoncomputer;
				transform.LookAt (laptop.transform);
				GetComponent<Animator> ().SetTrigger ("comp");
			}
			break;
		
		case XanaxGirlState.turnoncomputer:
			//play anim of turning on computer and say some shit
			timerforturningoncomputer += Time.deltaTime;
			if (timerforturningoncomputer > 7)
				thisState = XanaxGirlState.dancing;
			//switch to dancing
			break;


		case XanaxGirlState.dancing:
			//approach player until close and then turn around and switch to dancing mode

			//do this for a bout 30 sec, if player ever walks away approach him again
			//switch to dying mode
			break;

		case XanaxGirlState.dying:
			//death anim is playing, dont doanything except prepare for end of this sequence
			break;
		}
	}

	IEnumerator switchbacktolookingforxanax() {
		yield return new WaitForSeconds (3f);
		currentXanWaypoint.thisXanaxPickup.gameObject.AddComponent<Rigidbody> ();
		currentXanWaypoint.thisXanaxPickup.gameObject.AddComponent<CapsuleCollider> ();

		thisState = XanaxGirlState.lookingforxanax;
		currentXanWaypoint.thisXanaxPickup.transform.parent = null;
		GetComponent<Animator> ().SetTrigger ("walk");

	}
}
