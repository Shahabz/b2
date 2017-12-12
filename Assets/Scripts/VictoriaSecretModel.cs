using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class VictoriaSecretModel : MonoBehaviour, IInteractable {

	public enum ModelState {Regular, Combat, Idle};
	public ModelState thisModelState = ModelState.Regular;
	NavMeshAgent thisNavMeshAgent;
	public Vector3 navMeshTarget;
	public Transform target;
	GameObject clif;

	Vector3 cliffordPosition;

	bool hasTalked;

	bool bSwitchToWalking, bSwitchToStanding, bSwitchToFiring;
	public bool dontLookAtOnInteract;
	public ParticleSystem MuzzleFlash;
	float ft = 4, ftr = -1;

	void Start () {
		clif = GameObject.FindGameObjectWithTag ("CLIFFORD");

		thisNavMeshAgent = GetComponent<NavMeshAgent> ();
		MuzzleFlash = GetComponentInChildren<ParticleSystem> (true);
		if (MuzzleFlash)MuzzleFlash.gameObject.SetActive (false);
		if (target) {
			navMeshTarget = target.position;
			thisNavMeshAgent.SetDestination (target.position);

		}

	}
	
	// Update is called once per frame
	void Update () {
		switch (thisModelState) {
		case ModelState.Combat:
			if (clif) transform.LookAt (clif.transform.position);
			if (ftr >= 0) {
				ftr += Time.deltaTime;
				if (ftr > ft) {
					ftr = -1;
					SwitchToStanding ();
				}
			}
			break;
		
		case ModelState.Regular:
			if (bSwitchToWalking) {
				bSwitchToWalking = false;
			}
			if (bSwitchToStanding) {
				SwitchToStanding ();
				bSwitchToStanding = false;
			}
			if (bSwitchToFiring) {
				SwitchToFiring ();
				bSwitchToFiring = false;
			}
			if (Vector3.Distance (transform.position, target.position) < 5f) {
				bSwitchToFiring = true;
			}
			break;
		}
	}

	public void Interact() {
		if (!dontLookAtOnInteract)
			transform.LookAt (TestPlayerController.s_instance.transform);
		if (!hasTalked && !dontLookAtOnInteract) {
			GetComponent<DialogueSystem> ().StartDialogue ();
			hasTalked = true;
		}
		if (dontLookAtOnInteract) {
			GetComponent<InteractEvent> ().Interact ();
			CatLogic[] temp = FindObjectsOfType<CatLogic> ();
			foreach (CatLogic x in temp)
				Destroy (x.gameObject);
		}
	}

	public void SwitchToFiring() {
		thisNavMeshAgent.isStopped = true;
		GetComponent<Animator> ().SetTrigger ("fire");
		if (MuzzleFlash)MuzzleFlash.gameObject.SetActive(true);
		ftr = 0;
		thisModelState = ModelState.Combat;
	}

	public void SwitchToStanding() {
		thisNavMeshAgent.isStopped = true;
		GetComponent<Animator> ().SetTrigger ("idle");
		if (MuzzleFlash)MuzzleFlash.gameObject.SetActive (false);


	}

	public void SwitchToWalking() {
		thisNavMeshAgent.SetDestination (target.position);
		GetComponent<Animator> ().SetTrigger ("run");
	}
}
