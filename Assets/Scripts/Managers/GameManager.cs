using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum DebugStartPosition {Intro, Computer, Tolstoy, Highway};

public class GameManager : MonoBehaviour {

	public int day = 0;
	public static GameManager s_instance;
	public DebugStartPosition thisDebugStartPosition = DebugStartPosition.Tolstoy;
	[SerializeField]
	Camera Intro, ComputerCam, Tolstoy, HighwayCam;

	[SerializeField]
	Transform IntroTransform, ComputerTransform, TolstoyTransform, HighwayTransform;

	public bool completeTherapy, saveWomen;
	public GameObject enableIfCompleteTherapy, enableIfSaveWomen, enableIfSaveWomen2;
	public void SetCompleteTherapy(bool isTrue) {
		completeTherapy = true;
		enableIfCompleteTherapy.SetActive (true);
	}

	public void SetSaveWomen(bool istrue) {
		saveWomen = true;
		enableIfSaveWomen.SetActive (true);
		enableIfSaveWomen2.SetActive (true);
	}

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
		if (saveWomen) {
			SetSaveWomen (true);
		}
		if (completeTherapy) {
			SetCompleteTherapy (true);
		}
		TestPlayerController.s_instance.gameObject.SetActive (true);
		switch (thisDebugStartPosition) {
		case DebugStartPosition.Computer:
			Camera.main.transform.position = ComputerCam.transform.position;
			Camera.main.transform.rotation = ComputerCam.transform.rotation;

			break;
		case DebugStartPosition.Intro:
				TestPlayerController.s_instance.transform.position = IntroTransform.position;
			break;

		case DebugStartPosition.Tolstoy:
                TestPlayerController.s_instance.transform.position = TolstoyTransform.position;
                break;

        case DebugStartPosition.Highway:
                Camera.main.transform.position = HighwayCam.transform.position;
                Camera.main.transform.rotation = HighwayCam.transform.rotation;
                PlayerController.s_instance.transform.position = HighwayTransform.position;
                break;
		}
	}

    public float timeBonus = 30f;

    public void AddTime()
    {
        second += timeBonus;

    }

	void GotoNextDay () {
		OnNextDay ();
		day++;
	}

    public float second = 300;
    public Text timeDisplay;//, hourDisplay, minuteDisplay;

    void Update()
    {
        //second -= Time.deltaTime;
        //timeDisplay.text = (ReturnSecond() < 10) ? timeDisplay.text = ReturnMinute().ToString() + ":0" + ReturnSecond().ToString() : timeDisplay.text = ReturnMinute().ToString() + ":" + ReturnSecond().ToString();
    }

    public int ReturnSecond()
    {
        return ((int)(second % 60));
    }

    public int ReturnMinute()
    {
        return (Mathf.FloorToInt(second / 60));
    }
}
