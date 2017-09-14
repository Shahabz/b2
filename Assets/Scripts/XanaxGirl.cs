using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XanaxGirl : GirlController {

	public XanaxWaypoint[] xanaxWaypoints;
	public Transform holdObject;
	XanaxWaypoint currentXanWaypoint;
	enum XanaxGirlState {normal, lookingforxanax, walkingtoxanax, taking_xanax, gotocomputer, turnoncomputer, dancing, dying};
	XanaxGirlState thisState = XanaxGirlState.normal;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		switch (thisState) {
		case XanaxGirlState.normal:

			break;

		case XanaxGirlState.lookingforxanax:
			foreach (XanaxWaypoint x in xanaxWaypoints) {
				if (x.hasBeenAccessed == false) {
					thisNavMeshAgent.SetDestination (x.transform.position);
					thisState = XanaxGirlState.walkingtoxanax;
				}
			}
			thisState = XanaxGirlState.turnoncomputer;
			break;

		case XanaxGirlState.walkingtoxanax:
			if (Vector3.Distance (currentXanWaypoint.transform.position, transform.position) < 1) {
				thisState = XanaxGirlState.taking_xanax;
			}
			break;

		case XanaxGirlState.taking_xanax:
			if (currentXanWaypoint.hasBeenAccessed == false) {
				currentXanWaypoint.thisXanaxPickup.transform.SetParent (holdObject);
				GetComponent<Animator> ().SetTrigger ("pill");
				currentXanWaypoint.hasBeenAccessed = true;
			}
			break;

		case XanaxGirlState.gotocomputer:
			//walk tocomputer

			//check if reached
			break;
		
		case XanaxGirlState.turnoncomputer:
			//play anim of turning on computer and say some shit

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
		thisState = XanaxGirlState.lookingforxanax;
		currentXanWaypoint.thisXanaxPickup.transform.parent = null;

	}
}
