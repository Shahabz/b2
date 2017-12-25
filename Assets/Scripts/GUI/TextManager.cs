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
		if (isShowingTypeWriterEffect) {
			TypeWrite ();
		}
	}
		
	public void SetNotification (string inString, float timeUntilDisable = 4f){
		notification.text = inString;
		notificationTime = timeUntilDisable;
		showNotification = true;
	}

	public void SetNotification (string inString)
	{
		notification.text = inString;
		notificationTime = 5;
		showNotification = true;
	}

	public void SetPromptForced(string thisString)
	{
		SetPrompt (thisString, 5f);
	}

	public void SetPrompt(string inString, float timeUntilDisable = 4f) {
		prompt.text = inString;
		promptTime = timeUntilDisable;
		showPrompt = true;
	}

	bool isShowingTypeWriterEffect;
	public bool GetIsTypeWriting(){
		return isShowingTypeWriterEffect;
	}

	//TYPE WRITER LOGIC

	string stringToDisplay;
	float typeCharTime = .03f, typeCharTimer; // speed of typewriter
	int typeCharIterator;
	private float timeElapsed = 0;   
	int thisWordCount;

	public void SetSubtitle(string inString) {
		subtitle.text = "";
		typeCharIterator = 0;
		timeElapsed = 0;
		isShowingTypeWriterEffect = true;
		stringToDisplay = inString;
	}

	void TypeWrite () {

		if (GenericTimer.RunGenericTimer(typeCharTime, ref typeCharTimer)) {
			//print (stringToDisplay);
			if (stringToDisplay.Length > typeCharIterator) {
				subtitle.text += stringToDisplay.ToCharArray ()[typeCharIterator];
				typeCharIterator++;
			} else {
				isShowingTypeWriterEffect = false;
			}
		}
	}

	public void CompleteTypeWrite() {
		isShowingTypeWriterEffect = false;
		subtitle.text = stringToDisplay;
	}

}
