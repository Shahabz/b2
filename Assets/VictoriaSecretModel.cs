using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoriaSecretModel : MonoBehaviour, IInteractable {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Interact() {
		transform.LookAt (TestPlayerController.s_instance.transform);
		GetComponent<DialogueSystem> ().StartDialogue ();
	}
}
