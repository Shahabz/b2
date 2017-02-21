using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public enum Jobs {None, Scopely, Survios, Within, VisionaryVR, SnapChat, Heavy_Iron};

public class JobText : MonoBehaviour {

	public List<string> allJobResponses = new List<string>();
	public TextAsset noResponse;
	public GameObject jobTextPrefab;

	public static JobText s_instance;

	void Awake() {
		if (s_instance == null) {
			s_instance = this;
		} else {
			Destroy (this);
		}
	}

	public void SpawnJobText () {
		GameObject temp = (GameObject)Instantiate (jobTextPrefab, transform.position, transform.rotation) as GameObject;
		temp.GetComponentInChildren<Text> ().text = GetJobDescription();
		temp.GetComponent<DestroyAfter> ().Lifetime = GetJobDescriptionScrollTime ();


	}

	public float GetJobDescriptionScrollTime () {
		return (float)GetJobDescription ().Length / 4;
	}

	string GetJobDescription() {
        string outString;
		Jobs thisJob = (Jobs)GameManager.s_instance.day;
		switch (thisJob) {
		case Jobs.Scopely:
			outString = "No Response.";
			break;
		default :
			outString = "No Response.";
			break;
		}
        return outString;
	}

}
