using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LethalObject : MonoBehaviour {
	public AudioSource spawn, retract;
	bool descending = true, ascending = false;
	float slamSpeed = .3f;
	Vector3 initPosition;
	[SerializeField]
	float descendDistance = 10f;
	// Use this for initialization
	void Start () {
		initPosition = transform.position;
		spawn.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		HandleDescent ();
		HandleAscent ();
	}

	public void RetractObject () {
		retract.Play ();
		ascending = true;
		descending = false;
	}

	void HandleDescent() {
		if (descending) {
			transform.position += Vector3.down * slamSpeed;
			if (Vector3.Distance (transform.position, initPosition) > descendDistance) {
				descending = false;
			}
		}
	}

	void HandleAscent () {
		if (ascending) {
			transform.position += Vector3.up * slamSpeed;
			if (transform.position.y > initPosition.y) {
				ascending = false;
				Destroy (gameObject, 3f);
			}
		}
	}
}
