﻿using System.Collections;
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
			transform.Translate (Vector3.Normalize (EndPoint.position - StartPoint.position) * travelSpeed);
			if (Vector3.Distance (transform.position, EndPoint.position) < .1f) {
				thisElevatorState = ElevatorState.PausingAtTop;
			}
			break;
		case ElevatorState.Descending:
			transform.Translate (Vector3.Normalize (StartPoint.position - EndPoint.position) * travelSpeed);
			if (Vector3.Distance (transform.position, StartPoint.position) < .1f) {
				thisElevatorState = ElevatorState.PausingAtBottom;
			}
			break;
		case ElevatorState.PausingAtTop:
			if (GenericTimer.RunGenericTimer (pauseTime, ref pauseTimer)) {
				thisElevatorState = ElevatorState.Descending;
			}
			break;
		case ElevatorState.PausingAtBottom:
			if (GenericTimer.RunGenericTimer (pauseTime, ref pauseTimer)) {
				thisElevatorState = ElevatorState.Ascending;
			}
			break;

		}
	}
}
