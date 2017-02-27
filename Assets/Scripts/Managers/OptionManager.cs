using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour {

	public static float FXVolume = 0.7f;
	public static float BGMVolume = 0.7f;
	public static float VOVolume = 0.7f;

	void Awake() {
		if(PlayerPrefs.HasKey("FXVolume")) {
			FXVolume = PlayerPrefs.GetFloat("FXVolume");
		} else {
			PlayerPrefs.SetFloat("FXVolume", FXVolume);
		}
		if(PlayerPrefs.HasKey("BGMVolume")) {
			FXVolume = PlayerPrefs.GetFloat("BGMVolume");
		} else {
			PlayerPrefs.SetFloat("BGMVolume", BGMVolume);
		}
		if(PlayerPrefs.HasKey("VOVolume")) {
			FXVolume = PlayerPrefs.GetFloat("VOVolume");
		} else {
			PlayerPrefs.SetFloat("VOVolume", VOVolume);
		}

		PlayerPrefs.Save();
	}

//	void Start () {
//		
//	}
//	
//	void Update () {
//		
//	}
}
