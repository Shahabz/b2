using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLookAtIK : MonoBehaviour {
	public RootMotion.FinalIK.LookAtIK thisLookat;
	public Transform target;
	public void ChangeLookAtIKTarget() {
		thisLookat.solver.target = target;
	}
}
