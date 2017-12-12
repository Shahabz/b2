using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTerminal : MonoBehaviour, IInteractable {

	public GameObject deathFX;
	// Use this for initialization
	bool isDestroyed;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isDestroyed) {
			transform.Translate (Vector3.up * -.01f);
		}
	}

	public void Interact () {
		if (!isDestroyed) {
			TestPlayerController.s_instance.gameObject.GetComponent<Animator> ().SetTrigger ("punch");
			TestPlayerController.s_instance.SetPlayerModeCutscene ();
			StartCoroutine ("SmashPause");
			GetComponentInChildren<Light> ().enabled = false;
			Instantiate (deathFX, transform.position, Quaternion.identity);
			GameObject.FindObjectOfType<GateTerminal> ().BlueTerminalDestroyed ();
			GetComponent<Collider> ().enabled = false;
			Destroy (gameObject, 5f);
			isDestroyed = true;
		}
	}

	IEnumerator SmashPause() {
		yield return new WaitForSeconds (1.2f);
		TestPlayerController.s_instance.SetPlayerModeNormal ();

	}
}
