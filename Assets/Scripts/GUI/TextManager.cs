using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

	public Text notification, prompt, subtitle;
	float notificationTime, notificationTimer;
	float promptTime, promptTimer;
	bool showNotification, showPrompt;
	public static TextManager s_instance;

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

	void Update () {
		if (showNotification) {
			if (GenericTimer.RunGenericTimer (notificationTime, ref notificationTimer)) {
				showNotification = false;
				notification.text = "";

			}
		}

		if (showPrompt) {
			if (GenericTimer.RunGenericTimer (promptTime, ref promptTimer)) {
				showPrompt = false;
				prompt.text = "";

			}
		}
	}
		
	public void SetNotification (string inString, float timeUntilDisable){
		notification.text = inString;
		notificationTime = timeUntilDisable;
		showNotification = true;
	}

	public void SetPrompt(string inString, float timeUntilDisable = 2f) {
		prompt.text = inString;
		promptTime = timeUntilDisable;
		showPrompt = true;
	}

	public void SetSubtitle(string inString) {
	
	}


}
