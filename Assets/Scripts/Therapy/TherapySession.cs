using UnityEngine;
using System.Collections.Generic;

//This class is used by DialogueAsset.cs to instantiate custom dialogue assets which are used to store the information for each chat level
public class TherapySession: ScriptableObject
{
	public List<TherapySessionElement> therapySessionElements; //holds a list of dialogue items which contains the strings and info needed for a level of the chat interface
}