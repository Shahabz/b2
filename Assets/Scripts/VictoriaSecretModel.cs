using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class VictoriaSecretModel : MonoBehaviour, IInteractable {


	NavMeshAgent thisNavMeshAgent;
	public Vector3 navMeshTarget;
	public Transform target;

	Vector3 cliffordPosition;

	bool bSwitchToWalking, bSwitchToStanding, bSwitchToFiring;
	public bool dontLookAtOnInteract;

	void Start () {
		thisNavMeshAgent = GetComponent<NavMeshAgent> ();
		if (target) {
			print ("HIT");
			navMeshTarget = target.position;
			bSwitchToWalking = true;
			SwitchToWalking ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (bSwitchToWalking) {
			bSwitchToWalking = false;
		}
		if (bSwitchToStanding) {
			bSwitchToStanding = false;
		}
		if (bSwitchToFiring) {
			SwitchToFiring ();
			bSwitchToFiring = false;
		}
		if (Vector3.Distance (transform.position, target.position) < 3f) {
			bSwitchToFiring = true;
		}
	}

	public void Interact() {
		if (!dontLookAtOnInteract)
	 		transform.LookAt (TestPlayerController.s_instance.transform);
		GetComponent<DialogueSystem> ().StartDialogue ();
	}

	public void SwitchToFiring() {
		thisNavMeshAgent.isStopped = true;
		GetComponent<Animator> ().SetTrigger ("fire");
		cliffordPosition = GameObject.FindGameObjectWithTag ("CLIFFORD").transform.position;
		transform.LookAt (cliffordPosition);
	}

	public void SwitchToStanding() {
		thisNavMeshAgent.isStopped = true;
		GetComponent<Animator> ().SetTrigger ("idle");

	}

	public void SwitchToWalking() {
		thisNavMeshAgent.SetDestination (target.position);
		GetComponent<Animator> ().SetTrigger ("run");
	}
}
