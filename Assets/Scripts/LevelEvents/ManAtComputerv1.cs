using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ManAtComputerv1 : MonoBehaviour {
	NavMeshAgent thisNavMeshAgent;
	public Transform escapePoint, dodgePoint;
	bool isEscapeAround, isEscapeDirect;
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
		StartCoroutine (ReactThenEscapeDirect ());
	}

	public void EscapeAround() {
		isEscapeAround = true;
		StartCoroutine (ReactThenDodge ());
	}

	IEnumerator ReactThenEscapeDirect() {
		transform.LookAt (TestPlayerController.s_instance.transform);
		yield return new WaitForSeconds (.5f);
		thisNavMeshAgent.SetDestination (escapePoint.position);

	}

	IEnumerator ReactThenDodge () {
		transform.LookAt (TestPlayerController.s_instance.transform);
		yield return new WaitForSeconds (.5f);
		thisNavMeshAgent.SetDestination (dodgePoint.position);
	}


	void CheckDistToDodgePoint() {
		if (Vector3.Distance (transform.position, dodgePoint.position) < 1f) {
			isEscapeAround = false;
			EscapeDirect ();
		}
	}

	void CheckDistToEscapePoint () {
		if (Vector3.Distance (transform.position, escapePoint.position) < 1f) {
			Destroy (gameObject);
		}
	}
}
