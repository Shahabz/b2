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
	bool isShowingTypeWriterEffect;
	public string textShownOnScreen;
	public string typeWriterText = "The text you want shown on screen with typewriter effect.";
	public float wordsPerSecond = 2, wordTimer; // speed of typewriter
	private float timeElapsed = 0;   
	int thisWordCount;

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
			if (GenericTimer.RunGenericTimer(wordsPerSecond, ref wordTimer)) {
				//textShownOnScreen = GetWords (typeWriterText, thisWordCount);
				notification.text = textShownOnScreen;
			}
		}
	}
		
	public void SetNotification (string inString, float timeUntilDisable = 2f){
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

		timeElapsed = 0;
		isShowingTypeWriterEffect = true;
		typeWriterText = inString;
		//thisWordCount = inString.Split(new string[] {" "}, 1000000).Length;

	}




	private string GetWords(string text)
	{
		// loop through each character in text
		for (int i = 0; i < text.Length; i++)
		{ 
			if (text[i] == ' ')
			{
				thisWordCount--;
			}
			if (thisWordCount <= 0)
			{
				return text.Substring(0, i);
			}
		}
		return text;
	}


}
