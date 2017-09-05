using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is the cat used in the whodunit mini game
public class WhodunitCat : CatLogic, IInteractable {

	CatWhoDunitManager myManagerRef;
	public string thisCatStory;
	bool hasPlayerHeardThisStory;
	bool inHostageMode;
	[SerializeField]
	bool isEvilCat;

	// Use this for initialization
	void Start () {
		base.Start ();
		myManagerRef = FindObjectOfType<CatWhoDunitManager> ();
		GetComponent<DialogueSystem> ().onDialogueEnd.AddListener (OnDialogueEnd);
		TestPlayerController.s_instance.InteractiveCutscene_Interact.AddListener (ReleaseHostage);
		TestPlayerController.s_instance.InteractiveCutscene_Fire.AddListener(KillCat);

	}
	
	void Update() {
		base.Update ();

	}

	public string myFlashbackStory;

	public void Interact() {
		if (thisCatState == CatStates.Waypoints && TestPlayerController.s_instance.thisPlayerMode == PlayerMode.Normal) {
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
				GetComponent<DialogueSystem> ().StartDialogue ();
				hasPlayerHeardThisStory = true;
			}
		}
	}

	public void HoldMeHostage() {
		TestPlayerController.s_instance.HoldCatHostage ();
		thisSkinnedMeshRenderer.enabled = false;

	}

	public void OnDialogueEnd() {
		HoldMeHostage ();
	}



	public void ReleaseHostage () {
		print ("ReleaseHostage");
		thisSkinnedMeshRenderer.enabled = true;
		TestPlayerController.s_instance.ReleaseCatHostage ();
		SwitchToState (CatStates.Waypoints);

	}

	public void KillCat() {
		if (isEvilCat) {

		} else {

		}
	}

}
