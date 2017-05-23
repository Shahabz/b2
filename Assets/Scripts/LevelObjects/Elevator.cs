using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

	enum ElevatorState {Ascending, Descending, PausingAtTop,PausingAtBottom};
	[SerializeField]
	ElevatorState thisElevatorState = ElevatorState.Ascending;

	[SerializeField]
	Transform StartPoint, EndPoint;


	[SerializeField]
	float travelSpeed, pauseTime;
	float pauseTimer;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		switch (thisElevatorState) {
		case ElevatorState.Ascending:

			break;
		case ElevatorState.Descending:

			break;
		case ElevatorState.PausingAtTop:

			break;
		case ElevatorState.PausingAtBottom:

			break;

		}
	}
}
