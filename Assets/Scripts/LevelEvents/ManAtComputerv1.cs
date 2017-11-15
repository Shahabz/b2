using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ManAtComputerv1 : MonoBehaviour {
	enum DavidState {waiting, running};
	NavMeshAgent thisNavMeshAgent;
	public Transform escapePoint, dodgePoint;
	bool isEscapeAround, isEscapeDirect;
	public bool hasReaction = true;
	float distanceToToggleNextWaypoint = 4f;
	bool whichDirection;
	// Use this for initialization
	void Start () {
		thisNavMeshAgent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isEscapeAround) {
			CheckDistToDodgePoint ();
		}

		if (isEscapeDirect) {
			CheckDistToEscapePoint ();
		}
	}

	public void React(bool goDirect) {
		whichDirection = goDirect;
		transform.LookAt (TestPlayerController.s_instance.transform);

		if (GetComponent<Animator>()!=null)
			GetComponent<Animator> ().SetTrigger ("alert");
		
		GetComponent<DialogueSystem> ().StartDialogue ();
	}

	public void RunAway() {
		if (whichDirection)
			EscapeDirect ();
		else
			EscapeAround ();
	}

	public void EscapeDirect(){
		isEscapeDirect = true;
		if (hasReaction) {

			StartCoroutine (ReactThenEscapeDirect ());
		} else {
			thisNavMeshAgent.SetDestination (escapePoint.position);
		}
	}

	public void EscapeAround() {
		isEscapeAround = true;

		StartCoroutine (ReactThenDodge ());
	}

	IEnumerator ReactThenEscapeDirect() {
		yield return new WaitForSeconds (.5f);
		thisNavMeshAgent.SetDestination (escapePoint.position);
		if (GetComponent<Animator>()!=null)
			GetComponent<Animator> ().SetTrigger ("run");
	}

	IEnumerator ReactThenDodge () {
		transform.LookAt (TestPlayerController.s_instance.transform);
		yield return new WaitForSeconds (.5f);
		thisNavMeshAgent.SetDestination (dodgePoint.position);
		if (GetComponent<Animator>()!=null)
			GetComponent<Animator> ().SetTrigger ("run");                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
	}


	void CheckDistToDodgePoint() {
		if (Vector3.Distance (transform.position, dodgePoint.position) < distanceToToggleNextWaypoint) {
			isEscapeAround = false;
			isEscapeDirect = true;
			thisNavMeshAgent.SetDestination (escapePoint.position);
		}
	}

	void CheckDistToEscapePoint () {
		if (Vector3.Distance (transform.position, escapePoint.position) < distanceToToggleNextWaypoint) {
			Destroy (gameObject);
		}
	}
}
