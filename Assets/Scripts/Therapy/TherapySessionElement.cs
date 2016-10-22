using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class TherapySessionElement
{
	public AudioSource question;
	public List<AudioSource> answersSpoken;
	public List<string> answerChoices;
	public AudioSource response;

	public string correctAnswer; //list of sentences that are appropiate formations of wordChoices
}
