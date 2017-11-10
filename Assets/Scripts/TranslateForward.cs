using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateForward : MonoBehaviour {

	public float translateSpeed = .001f;
	public float duration;
	bool isTranslating = false;
	public bool autoTranslate;
	// Update is called once per frame
	void Update () {
		if (isTranslating)
			transform.Translate (Vector3.forward*translateSpeed);

		if (autoTranslate) {
			StartTranslation ();
			autoTranslate = false;
		}
	}

	public void StartTranslation() {
		StartCoroutine (Translate ());
	}
	IEnumerator Translate() {
		isTranslating = true;
		yield return new WaitForSeconds (duration);
		isTranslating = false;
	}
}
