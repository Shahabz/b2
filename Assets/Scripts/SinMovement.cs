using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MonoBehaviour {
	float originalY;
	float randomDelay;
	float timer;
	float dtSum;
	public float floatStrength = 10f;
	// Use this for initialization
	void Start () {
		originalY = transform.position.y;
		randomDelay = Random.Range (0f, 3f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (timer < randomDelay) {
			timer += Time.deltaTime;
			return;
		}
		dtSum += Time.deltaTime;
		transform.position = new Vector3(transform.position.x,
			originalY + ((float)Mathf.Sin(dtSum) * floatStrength),
			transform.position.z);
	}
}
