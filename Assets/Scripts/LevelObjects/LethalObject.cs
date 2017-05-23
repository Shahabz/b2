using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalObject : MonoBehaviour {
	public AudioSource spawn, retract;

	// Use this for initialization
	void Start () {
		spawn.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RetractObject () {
		retract.Play ();
	}
}
