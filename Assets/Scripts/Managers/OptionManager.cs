using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour {

	public static float FXVolume = 0.7f;
	public static float BGMVolume = 0.7f;
	public static float VOVolume = 0.7f;

	static bool isDirty = false;

	void Awake() {
		if(PlayerPrefs.HasKey("FXVolume")) {
			FXVolume = PlayerPrefs.GetFloat("FXVolume");
		} else {
			PlayerPrefs.SetFloat("FXVolume", FXVolume);
		}
		if(PlayerPrefs.HasKey("BGMVolume")) {
			BGMVolume = PlayerPrefs.GetFloat("BGMVolume");
		} else {
			PlayerPrefs.SetFloat("BGMVolume", BGMVolume);
		}
		if(PlayerPrefs.HasKey("VOVolume")) {
			VOVolume = PlayerPrefs.GetFloat("VOVolume");
		} else {
			PlayerPrefs.SetFloat("VOVolume", VOVolume);
		}

		PlayerPrefs.Save();
	}

	public static void SetDirty() {
		isDirty = true;
	}

//	void Start () {
//		
//	}
//	
	void Update () {
		if(isDirty) {
			PlayerPrefs.SetFloat("FXVolume", FXVolume);
			PlayerPrefs.SetFloat("BGMVolume", BGMVolume);
			PlayerPrefs.SetFloat("VOVolume", VOVolume);

			PlayerPrefs.Save();
			isDirty = false;
		}
	}
}
