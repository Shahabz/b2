using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XanaxGirl : GirlController {

	public XanaxWaypoint[] xanaxWaypoints;
	public Transform holdObject;
	XanaxWaypoint currentXanWaypoint;
	public enum XanaxGirlState {normal, lookingforxanax, walkingtoxanax, taking_xanax, gotocomputer, turnoncomputer, dancing, walktowardplayer, dying, lookatplayer, ghost};
	public XanaxGirlState thisState = XanaxGirlState.normal;
	bool SwitchToComputerState;
	public Transform computerWaypoint, laptop;	
	public GenericTriggerEvent onDancePartyStart;
	float timerforturningoncomputer, dancetimer;
	public GenericTriggerEvent thisEvent;
	public bool turnBodyTowardPlayer;
	public Transform ghostTransform;
	public GameObject Strobe;
	public AudioSource eerie; 
	public AudioSource collapse;
	public AudioSource drink;
	public AudioSource gag;
	public AudioSource Luigi;
	public GameObject[] Lights;
	bool hasDied;
	public bool isParty;
	public void SwitchToXanaxSearch () {
		thisState = XanaxGirlState.lookingforxanax;
		GetComponent<Animator> ().SetTrigger ("walk");
		GetComponent<AudioSource> ().Play ();//walking



	}

	void Start () {
		//xanaxWaypoints = FindObjectsOfType<XanaxWaypoint> ();
		if (ghostTransform!=null)
			GetComponent<Animator> ().SetTrigger ("walk");
		if (isParty)TextManager.s_instance.SetNotification ("Take Sally to the Party Next Door and Turn Up", 6f);

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
					GetComponent<RootMotion.FinalIK.LookAtIK> ().enabled = false;
					return;
				}
			}
			thisState = XanaxGirlState.gotocomputer;
			TextManager.s_instance.SetNotification ("* Sally took too much Xanax *", 4f);
			TestPlayerController.s_instance.GetComponent<HealthHandler> ().TakeStress (5);
			break;

		case XanaxGirlState.walkingtoxanax:
			thisNavMeshAgent.SetDestination (currentXanWaypoint.transform.position);
			if (Vector3.Distance (currentXanWaypoint.transform.position, transform.position) < .1) {
				if (currentXanWaypoint.hasBeenAccessed == false) {
					thisState = XanaxGirlState.taking_xanax;
					GetComponent<AudioSource> ().Stop ();//walking
					drink.Play();
					gag.Play ();
					GetComponent<Animator> ().ResetTrigger ("walk");
					StartCoroutine ("switchbacktolookingforxanax");
					currentXanWaypoint.thisXanaxPickup.transform.SetParent (holdObject);
					transform.LookAt (currentXanWaypoint.thisXanaxPickup.transform);
					transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0);
					currentXanWaypoint.thisXanaxPickup.transform.localPosition = Vector3.zero;
					currentXanWaypoint.thisXanaxPickup.transform.localRotation = Quaternion.identity;
					GetComponent<Animator> ().SetTrigger ("pill");
					currentXanWaypoint.hasBeenAccessed = true;
				} else {
					thisState = XanaxGirlState.lookingforxanax;

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
				GetComponent<AudioSource> ().Stop ();//walking

				transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0);
				GetComponent<Animator> ().SetTrigger ("comp");
			}
			break;
		
		case XanaxGirlState.turnoncomputer:
			//play anim of turning on computer and say some shit
			timerforturningoncomputer += Time.deltaTime;
			if (timerforturningoncomputer > 7) {
				thisState = XanaxGirlState.walktowardplayer;
				GetComponent<Animator> ().SetTrigger ("walk");
				FindObjectOfType<StrobeLight> ().enabled = true;
				FindObjectOfType<StrobeLight> ().gameObject.GetComponent<AudioSource> ().Play ();
				Strobe.SetActive (true);
				foreach (GameObject x in Lights)
					x.SetActive (false);
			}
			//switch to dancing
			break;


		case XanaxGirlState.dancing:
			dancetimer += Time.deltaTime;
			if (dancetimer > 30) {
				GetComponent<Animator> ().SetTrigger ("die");
				hasDied = true;
				thisState = XanaxGirlState.dying;
				SoundtrackManager.s_instance.FadeOut(Luigi);
				GetComponent<AudioSource> ().Stop ();//walking
				collapse.Play();
				eerie.Play ();
			}
			//approach player until close and then turn around and switch to dancing mode
			if (Vector3.Distance (TestPlayerController.s_instance.transform.position, transform.position) > 5f) {
				GetComponent<Animator> ().SetTrigger ("walk");
				thisState = XanaxGirlState.walktowardplayer;
				thisNavMeshAgent.isStopped = false;
				GetComponent<AudioSource> ().Play ();//walking

			}
			//do this for a bout 30 sec, if player ever walks away approach him again
			//switch to dying mode
			break;

		case XanaxGirlState.walktowardplayer:
			dancetimer += Time.deltaTime;
			thisNavMeshAgent.SetDestination (TestPlayerController.s_instance.transform.position);
			if (Vector3.Distance (TestPlayerController.s_instance.transform.position, transform.position) < 2f){
				GetComponent<Animator> ().SetTrigger ("dance");
				GetComponent<Animator> ().ResetTrigger ("walk");
				GetComponent<AudioSource> ().Stop ();//walking

				thisState = XanaxGirlState.dancing;
		
				thisNavMeshAgent.isStopped = true;

			}
				
			break;

		case XanaxGirlState.dying:
			//death anim is playing, dont doanything except prepare for end of this sequence
			break;

		case XanaxGirlState.lookatplayer:
			if(turnBodyTowardPlayer)transform.LookAt(TestPlayerController.s_instance.transform.position);
			break;

		case XanaxGirlState.ghost:
			thisNavMeshAgent.SetDestination (ghostTransform.position);
			if (Vector3.Distance (transform.position, ghostTransform.position)< 3f) {
				Destroy (gameObject);
			}
			break;
		}
	}

	IEnumerator switchbacktolookingforxanax() {
		yield return new WaitForSeconds (1.8f);
		currentXanWaypoint.thisXanaxPickup.gameObject.AddComponent<Rigidbody> ();
		currentXanWaypoint.thisXanaxPickup.gameObject.AddComponent<CapsuleCollider> ();

		thisState = XanaxGirlState.lookingforxanax;
		currentXanWaypoint.thisXanaxPickup.transform.parent = null;
		GetComponent<Animator> ().SetTrigger ("walk");
		GetComponent<AudioSource> ().Play ();//walking

	}

	public void ForceDie(){
		if (!hasDied) {
			GetComponent<Animator> ().SetTrigger ("die");
			SoundtrackManager.s_instance.FadeOut (Luigi);
			thisState = XanaxGirlState.dying;
			GetComponent<AudioSource> ().Stop ();//walking
			collapse.Play ();
			eerie.Play ();
		}
	}
}
