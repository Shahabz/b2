using UnityEngine;
using System.Collections;

public class TranslateUp : MonoBehaviour {
	public float translateSpeed = .001f;	
	// Update is called once per frame
	public bool isTranslating = true;

	void Update () {
		if (isTranslating) {
			transform.Translate (Vector3.up * translateSpeed);
		}
	}

	public void StartTranslation() {
		isTranslating = true;
	}
}
