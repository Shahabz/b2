using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GirlController : MonoBehaviour {

	enum GirlState {Following, Idle};
	GirlState thisGirlState = GirlState.Idle;

	protected float distanceToTriggerFollow = 20f;
	public float stoppingDistance = 2f;

	[SerializeField]
	protected NavMeshAgent thisNavMeshAgent;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	protected void Update () {
		switch (thisGirlState) {
		case GirlState.Following:
			CheckIsPlayerFarOrTooClose ();
			thisNavMeshAgent.isStopped = false;
			thisNavMeshAgent.SetDestination (TestPlayerController.s_instance.transform.position);

			break;

		case GirlState.Idle:
			CheckIsPlayerClose ();
			thisNavMeshAgent.isStopped = true;
			break;
		}
	}

	protected void CheckIsPlayerClose () {
		if (Vector3.Distance (TestPlayerController.s_instance.transform.position, transform.position) < distanceToTriggerFollow
			&& Vector3.Distance (TestPlayerController.s_instance.transform.position, transform.position) > stoppingDistance) {
			GetComponent<Animator> ().SetTrigger ("walk");
			thisGirlState = GirlState.Following;
			if (GetComponent<AudioSource>())GetComponent<AudioSource> ().Play ();//walking

		}
	}

	protected void CheckIsPlayerFarOrTooClose() {
		if (Vector3.Distance (TestPlayerController.s_instance.transform.position, transform.position) > distanceToTriggerFollow) {
			GetComponent<Animator> ().SetTrigger ("idle");
			thisGirlState = GirlState.Idle;
			if (GetComponent<AudioSource>())GetComponent<AudioSource> ().Stop ();//walking

		} else if (Vector3.Distance (TestPlayerController.s_instance.transform.position, transform.position) < stoppingDistance){
			GetComponent<Animator> ().SetTrigger ("idle");
			thisGirlState = GirlState.Idle;
			if (GetComponent<AudioSource>())GetComponent<AudioSource> ().Stop ();//walking

		}
	}


}
