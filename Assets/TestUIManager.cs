using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIManager : MonoBehaviour {

	public enum UIState {
		None, Menu
	}

	UIState state;

	[SerializeField]
	GameObject menu;

//	void Start () {
//		
//	}
//
/*
	void Update () {
		switch(state) {
			case UIState.None:
				Cursor.lockState = CursorLockMode.Locked;

				if(NPInputManager.input.Menu.WasPressed) {
					ToggleMenu(true);
					return;
				}
				break;
			case UIState.Menu:
				Cursor.lockState = CursorLockMode.Confined;
				if(NPInputManager.input.Menu.WasPressed) {
					ToggleMenu(false);
					return;
				}
				break;
			default:
				break;
		}
	}
*/
	void ToggleMenu(bool show) {
		if(show) {
			state = UIState.Menu;
			menu.SetActive(true);
			TestPlayerController.s_instance.lockInput = true;
		} else {
			state = UIState.None;
			menu.SetActive(false);
			TestPlayerController.s_instance.lockInput = false;
		}
	}

	public void UpdateFXVolume(float volume) {
		OptionManager.FXVolume = volume;
	}

	public void UpdateMusicVolume(float volume) {
		OptionManager.BGMVolume = volume;
	}

	public void UpdateDialogueVolume(float volume) {
		OptionManager.VOVolume = volume;
	}
}
