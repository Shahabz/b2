using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

	void Start() {
		transform.Find("Menu/Volume/FX/Slider").GetComponent<Slider>().value = OptionManager.FXVolume;
		transform.Find("Menu/Volume/Music/Slider").GetComponent<Slider>().value = OptionManager.BGMVolume;
		transform.Find("Menu/Volume/VO/Slider").GetComponent<Slider>().value = OptionManager.VOVolume;
		SetState (UIState.None);
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
			//SceneManager.LoadScene (SceneManager.GetActiveScene().name); 
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
				//Cursor.lockState = CursorLockMode.Confined;
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

		if(state == TestUIManager.UIState.None) {
			TestPlayerController.s_instance.lockInput = TestPlayerController.InputLock.Unlocked;
		} else if(state == TestUIManager.UIState.Menu) {
			TestPlayerController.s_instance.lockInput = TestPlayerController.InputLock.Locked;
		} else if(state == TestUIManager.UIState.Cutscene) {
			TestPlayerController.s_instance.lockInput = TestPlayerController.InputLock.Locked;
		}
	}

	public void UpdateFXVolume(float volume) {
		OptionManager.FXVolume = volume;
		OptionManager.SetDirty();
	}

	public void UpdateMusicVolume(float volume) {
		OptionManager.BGMVolume = volume;
		OptionManager.SetDirty();
	}

	public void UpdateDialogueVolume(float volume) {
		OptionManager.VOVolume = volume;
		OptionManager.SetDirty();
	}
}
