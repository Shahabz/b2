using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ManAtComputerv2 : MonoBehaviour {
	NavMeshAgent thisNavMeshAgent;
	public Transform escapePoint, dodgePoint;
	bool isEscapeAround, isEscapeDirect;
	public bool hasReaction = true;
	float distanceToToggleNextWaypoint = 4f;
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

	public void EscapeDirect(){
		isEscapeDirect = true;
		if (hasReaction) {
			if (GetComponent<Animator>()!=null)
				GetComponent<Animator> ().SetTrigger ("alert");
			StartCoroutine (ReactThenEscapeDirect ());
		} else {
			thisNavMeshAgent.SetDestination (escapePoint.position);
		}
	}

	public void EscapeAround() {
		isEscapeAround = true;
		if (GetComponent<Animator>()!=null)
			GetComponent<Animator> ().SetTrigger ("alert");
		StartCoroutine (ReactThenDodge ());
	}

	IEnumerator ReactThenEscapeDirect() {
		yield return new WaitForSeconds (.5f);
		thisNavMeshAgent.SetDestination (escapePoint.position);
		if (GetComponent<Animator>()!=null)
			GetComponent<Animator> ().SetTrigger ("run");
	}

	IEnumerator ReactThenDodge () {
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
		}
	}
}
