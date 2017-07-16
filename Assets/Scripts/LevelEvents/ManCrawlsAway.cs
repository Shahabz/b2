using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ManCrawlsAway : MonoBehaviour {
	NavMeshAgent thisNavMeshAgent;
	public Transform escapePoint, dodgePoint;
	bool isEscapeAround, isEscapeDirect;
	public bool pauseAnim = false;
	float distanceToToggleNextWaypoint = 4f;
	// Use this for initialization
	void Start () {
		thisNavMeshAgent = GetComponent<NavMeshAgent> ();
		if (pauseAnim) {
			GetComponent<Animator> ().enabled = false;
		}
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
		if (pauseAnim) {
			GetComponent<Animator> ().enabled = true;
		}
		isEscapeDirect = true;
		thisNavMeshAgent.SetDestination (escapePoint.position);
		GetComponent<Animator> ().SetTrigger ("run");
	}

	public void EscapeAround() {
		thisNavMeshAgent = GetComponent<NavMeshAgent> ();
		isEscapeAround = true;
		thisNavMeshAgent.SetDestination (dodgePoint.position);
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
