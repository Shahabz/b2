using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour {

	public int day = 1;
	public static GameManager s_instance;
	// Use this for initialization
	void Start () {
	
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
