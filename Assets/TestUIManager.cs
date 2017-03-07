using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUIManager : MonoBehaviour {

	public enum UIState {
		None, Menu, Cutscene
	}

	UIState state;
	UIState lastState; //To know where to return to when done with menu
	public UIState State { get { return state; } }

	[SerializeField]
	GameObject menu;

	public static TestUIManager instance;

	void Awake () {
		instance = this;
	}

	void Update () {
		switch(state) {
			case UIState.None:
				break;
			case UIState.Menu:
				break;
			default:
				break;
		}

		if(NPInputManager.input.Menu.WasPressed) {
			ToggleMenu();
		}

		HandleCursorLock();
	}

	void ToggleMenu() {
		if(State == UIState.Menu) {
			state = lastState;
			menu.SetActive(false);
			TestPlayerController.s_instance.lockInput = TestPlayerController.InputLock.Unlocked;
		} else {
			state = UIState.Menu;
			menu.SetActive(true);
			TestPlayerController.s_instance.lockInput = TestPlayerController.InputLock.Locked;
		}
	}

	void HandleCursorLock() {
		switch(TestPlayerController.s_instance.lockInput) {
			case TestPlayerController.InputLock.CameraOnly:
			case TestPlayerController.InputLock.Locked:
				Cursor.lockState = CursorLockMode.Confined;
				break;
			case TestPlayerController.InputLock.Unlocked:
				Cursor.lockState = CursorLockMode.Locked;
				break;
			default:
				break;
		}
	}

	public void SetState(UIState state) {
		lastState = this.state;
		this.state = state;
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
