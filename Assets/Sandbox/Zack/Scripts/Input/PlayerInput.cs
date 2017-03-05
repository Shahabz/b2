using UnityEngine;
using System.Collections;

using InControl;

public class PlayerInput : BaseInput {

    //was getting null ref on this sorry you can replace this it just didnt work calling the class directly
    NPInputManager thisNPInputManager;

    private void Start()
    {
        thisNPInputManager = GetComponent<NPInputManager>();
    }
    
	void Update () {
		shoot = thisNPInputManager.input.Fire.WasPressed;
		aim = thisNPInputManager.input.Aim.IsPressed;
		lookDir = thisNPInputManager.input.Look;

		if(aim) {
			moveDir = Vector3.zero;
			sprint = false;
			melee = thisNPInputManager.input.Melee.WasPressed;
		} else {
			moveDir = new Vector3(thisNPInputManager.input.Move.X, 0f, thisNPInputManager.input.Move.Y); //its an x,y vec EW
			sprint = thisNPInputManager.input.Sprint;
			melee = false;
		}
	}
}
