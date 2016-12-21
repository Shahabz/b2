using UnityEngine;
using System.Collections;

public enum DebugStartPosition {Intro, Computer, Tolstoy, Highway};

public class GameManager : MonoBehaviour {

	public int day = 0;
	public static GameManager s_instance;
	public DebugStartPosition thisDebugStartPosition = DebugStartPosition.Tolstoy;
	[SerializeField]
	Camera Intro, ComputerCam, Tolstoy, HighwayCam;

	[SerializeField]
	Transform IntroTransform, ComputerTransform, TolstoyTransform, HighwayTransform;

	public delegate void NextDay();
	public NextDay OnNextDay;

    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

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

        case DebugStartPosition.Highway:
                Camera.main.transform.position = HighwayCam.transform.position;
                Camera.main.transform.rotation = HighwayCam.transform.rotation;
                PlayerController.s_instance.transform.position = HighwayTransform.position;
                break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GotoNextDay () {
		OnNextDay ();
		day++;
	}
}
