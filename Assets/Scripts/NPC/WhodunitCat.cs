using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *Every WHODUNIT cat can be talked to 
 * it has a string which is its story
 * 
 * 
 * 
 * 
 * 
 */
public class WhodunitCat : CatLogic, IInteractable {

	CatWhoDunitManager myManagerRef;
	public string thisCatStory;
	bool hasPlayerHeardThisStory;

	// Use this for initialization
	void Start () {
		myManagerRef = FindObjectOfType<CatWhoDunitManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string myFlashbackStory;

	public void Interact() {
		if (hasPlayerHeardThisStory) {
			myManagerRef.OnInteractWithWhoDunitCat ();
		}
		if (myManagerRef.hasDisplayedPuzzlePrompt) {
			TextManager.s_instance.SetSubtitle (thisCatStory);
			hasPlayerHeardThisStory = true;
		}
	}
}
