using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is the cat used in the whodunit mini game
public class WhodunitCat : CatLogic {

	public CatWhoDunitManager myManagerRef;
	public string thisCatStory;
	bool hasPlayerHeardThisStory;
	bool inHostageMode;
	[SerializeField]
	bool isEvilCat;
	public AudioSource diebitch;

	// Use this for initialization
	void Start () {
		base.Start ();
		GetComponent<DialogueSystem> ().onDialogueEnd.AddListener (OnDialogueEnd);
		canCauseStress = false;
	}
	
	void Update() {
		base.Update ();

	}

	public string myFlashbackStory;

	public override void Interact() {
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
		TestPlayerController.s_instance.HoldCatHostage (gameObject);
		transform.GetChild(0).gameObject.SetActive(false);

	}

	public void OnDialogueEnd() {
		HoldMeHostage ();
	}



	public void ReleaseHostage () {
			//transform.GetChild(0).gameObject.SetActive(true);
		transform.GetChild(0).gameObject.SetActive(true);
		SwitchToState (CatStates.Waypoints);

	}

	public void KillCat() {
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.gunshot);
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.catkill1);
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.catkill3);

		if (isEvilCat) {
			myManagerRef.QuestSucceeded ();
		} else {
			myManagerRef.QuestFailed ();
		}
		Destroy (gameObject);
	}

	public void FailState() {

			SwitchToState (CatStates.Idle);
			canCauseStress = true;

	}

	public void WinState() {
		Destroy(gameObject);
	}
}
