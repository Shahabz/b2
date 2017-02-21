using UnityEngine;
using System.Collections;

public class PlayerInput : BaseInput {
	
	void Start () {
	
	}

	void Update () {
		aim = Input.GetButton("Fire2");
		shoot = Input.GetButtonDown("Fire1");

		if(aim) {
			dir = Vector3.zero;
			sprint = false;
		} else {
			dir = new Vector3 (Input.GetAxis ("Horizontal"), 0.0f, Input.GetAxis ("Vertical"));
			sprint = Input.GetButton("Sprint");
		}
	}
}
