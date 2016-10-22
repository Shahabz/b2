using UnityEngine;
using System.Collections;

public enum DebugStartPosition {Intro, Computer, Tolstoy};

public class GameManager : MonoBehaviour {

	public int day = 0;
	public static GameManager s_instance;
	public DebugStartPosition thisDebugStartPosition = DebugStartPosition.Tolstoy;
	[SerializeField]
	Camera Intro, ComputerCam, Tolstoy;

	[SerializeField]
	Transform IntroTransform, ComputerTransform, TolstoyTransform;

	// Use this for initialization
	void Start () {
		switch (thisDebugStartPosition) {
		case DebugStartPosition.Computer:
			Camera.main.transform.position = ComputerCam.transform.position;
			Camera.main.transform.rotation = ComputerCam.transform.rotation;
			PlayerController.s_instance.transform.position = ComputerTransform.position;

			break;
		case DebugStartPosition.Intro:

			break;

		case DebugStartPosition.Tolstoy:

			break;
		}
	}
	void Awake() {
		if (s_instance == null) {
			s_instance = this;
		} else {
			Destroy (this);
		}

	}
	// Update is called once per frame
	void Update () {
	
	}
}
