using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KissEvent : MonoBehaviour {

	[SerializeField]
	GameObject naughtyPGO, girlGO;

	Transform kissPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			naughtyPGO.SetActive (true);
			TestPlayerController.s_instance.gameObject.SetActive (false);
			naughtyPGO.GetComponent<Animator> ().SetTrigger ("kiss");
			girlGO.GetComponent<Animator> ().SetTrigger ("kiss");

		}

	}

}
