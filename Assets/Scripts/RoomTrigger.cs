using UnityEngine;
using System.Collections;

public class RoomTrigger : MonoBehaviour {

	public Transform cameraPos;

	void OnTriggerEnter() {
		Camera.main.transform.rotation = cameraPos.rotation;
		Camera.main.transform.position = cameraPos.position;
	}
}
