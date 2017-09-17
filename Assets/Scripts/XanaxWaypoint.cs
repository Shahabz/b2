using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XanaxWaypoint : MonoBehaviour {
	public bool hasBeenAccessed;
	// Use this for initialization
	public XanaxPickup thisXanaxPickup;

	void Start () {
		thisXanaxPickup.owner = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
