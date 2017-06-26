using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just a sample interactable. Interactables should inherit from IInteractble like in this example.
/// </summary>
public class SampleInteractable : MonoBehaviour, IInteractable {

	void Start () {
        if (gameObject.layer != LayerMask.NameToLayer("Interactable"))
        {
            Debug.LogError("Needs to be on interactable layer to interact with", this);
        }	
	}
//	
//	void Update () {
//		
//	}

    public void Interact() {
        Debug.LogError("Activated");
        //Activate something
    }
}
