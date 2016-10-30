using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class TherapySessionElement
{
	public string questionAudioPath;
	public List<string> answerChoices;
	public string responseAudioPath;
	public string responseString;
	public string questionString; 

	public string correctAnswer; //list of sentences that are appropiate formations of wordChoices
}
