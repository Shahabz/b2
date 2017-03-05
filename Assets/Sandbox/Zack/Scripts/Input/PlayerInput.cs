using UnityEngine;
using System.Collections;

using InControl;

public class PlayerInput : BaseInput {
	void Update () {
		shoot = NPInputManager.input.Fire.WasPressed;
		aim = NPInputManager.input.Aim.IsPressed;
		lookDir = NPInputManager.input.Look;

		if(aim) {
			moveDir = Vector3.zero;
			sprint = false;
			melee = NPInputManager.input.Melee.WasPressed;
		} else {
			moveDir = new Vector3(NPInputManager.input.Move.X, 0f, NPInputManager.input.Move.Y); //its an x,y vec EW
			sprint = NPInputManager.input.Sprint;
			melee = false;
		}
	}
}
