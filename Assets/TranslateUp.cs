using UnityEngine;
using System.Collections;

public class TranslateUp : MonoBehaviour {
	float translateSpeed = .001f;	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.up*translateSpeed);
	}
}
