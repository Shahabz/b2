using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapTherapyManager : MonoBehaviour {


	/*
	 * BPM/60 = BPS
	 * We will have a canvas that has 4 UI Measure elements each spaced at a beat apart, or quarter note apart, 
	 * Quarter note gets the beat so every second that passes, the floating Note objects will pass the distance between two UI Measure elements 
	 * When the play inputs a value, if the distance between the next note and the Final measure object is a small enough value, you succeed
	 * else the vocals cut out and a bark plays saying that you fucked up that section
	*/

	AudioSource currentRapBeat, currentVocalTrack;
	AudioSource[] barks;


	float currentSongBPM;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
