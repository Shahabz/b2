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
		base.Start ();
		myManagerRef = FindObjectOfType<CatWhoDunitManager> ();

	}
	
	void Update() {
		base.Update ();
	}

	public string myFlashbackStory;

	public void Interact() {
		if (thisCatState == CatStates.Waypoints) {
			thisCatAnimator.SetTrigger ("idle");
			thisCatState = CatStates.Talking;
			thisNavMeshAgent.isStopped = true;
			transform.LookAt (TestPlayerController.s_instance.transform);
			if (!myManagerRef.hasDisplayedPuzzlePrompt) {
				myManagerRef.DisplayPrompt (gameObject);
				return;
			}
			if (!hasPlayerHeardThisStory) {
				myManagerRef.CatCompleted ();
			}
			if (myManagerRef.hasDisplayedPuzzlePrompt) {
				TextManager.s_instance.SetSubtitle (thisCatStory);
				hasPlayerHeardThisStory = true;
			}
		}
	}
}
