﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class VictoriaSecretModel : MonoBehaviour, IInteractable {


	NavMeshAgent thisNavMeshAgent;
	public Vector3 navMeshTarget;

	bool bSwitchToWalking, bSwitchToStanding;

	void Start () {
		thisNavMeshAgent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (bSwitchToWalking) {
			bSwitchToWalking = false;
		}
		if (bSwitchToStanding) {
			bSwitchToStanding = false;
		}
	}

	public void Interact() {
		transform.LookAt (TestPlayerController.s_instance.transform);
		GetComponent<DialogueSystem> ().StartDialogue ();
	}

	public void SwitchToStanding() {
		thisNavMeshAgent.isStopped = true;
		GetComponent<Animator> ().SetTrigger ("idle");

	}

	public void SwitchToWalking() {
		thisNavMeshAgent.isStopped = false;
		thisNavMeshAgent.SetDestination (navMeshTarget);
		GetComponent<Animator> ().SetTrigger ("walk");
	}
}
