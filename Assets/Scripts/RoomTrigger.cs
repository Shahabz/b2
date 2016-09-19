using UnityEngine;
using System.Collections;

public class RoomTrigger : MonoBehaviour {

	public Transform cameraPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter() {
		Camera.main.transform.rotation = cameraPos.rotation;
		Camera.main.transform.position = cameraPos.position;
		Camera.main.fieldOfView = cameraPos.GetComponent<Camera> ().fieldOfView;
	}
}
