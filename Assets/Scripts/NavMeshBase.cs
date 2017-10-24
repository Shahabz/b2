using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBase : MonoBehaviour {


	public NavMeshAgent thisNavMeshAgent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	protected void Update () {
		thisNavMeshAgent.SetDestination (TestPlayerController.s_instance.transform.position);

	}
}
