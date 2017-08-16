using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateForward : MonoBehaviour {

	public float translateSpeed = .001f;
	public float duration;
	bool isTranslating;
	// Update is called once per frame
	void Update () {
		if (isTranslating)
			transform.Translate (Vector3.forward*translateSpeed);
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
