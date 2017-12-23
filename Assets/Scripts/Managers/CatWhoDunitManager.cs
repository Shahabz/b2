using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWhoDunitManager : MonoBehaviour {

	public bool hasDisplayedPuzzlePrompt;
	int catsCompleted = 0; //must complete five before win state can be activated, prevents random luck from succeeding
	GameObject thisCatTriggeredDisplayPrompt;
	public WhodunitCat[] WhoDunitCats;
	public GameObject[] VictoriaSecretModels;
	public Cinemachine.CinemachineVirtualCamera thisCineCamera;
	public AudioSource success;
	public AudioSource diebitch;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CatCompleted () {
		catsCompleted++;
	}

	public void DisplayPrompt(GameObject thisCatTriggeredIt) {
		GetComponent<DialogueSystem> ().StartDialogue ();
		thisCatTriggeredDisplayPrompt = thisCatTriggeredIt;
		thisCatTriggeredDisplayPrompt.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera> ().enabled = true;
		hasDisplayedPuzzlePrompt = true;
	}

	public void OnFinishPrompt() {
		thisCatTriggeredDisplayPrompt.GetComponent<WhodunitCat> ().SwitchToState (CatStates.Waypoints);
		thisCatTriggeredDisplayPrompt.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera> ().enabled = false;

	}

	public void QuestSucceeded() {
		//pop up all of the models of women and kill the man
		//the women just stand their, and if you walk into them, they turn into skeletons
		diebitch.PlayDelayed (1f);

		TextManager.s_instance.SetNotification ("You saved the models", 6f);
		success.Play ();
		GameManager.s_instance.SetSaveWomen (true);
		foreach (WhodunitCat x in WhoDunitCats) {
			x.WinState ();
		}
		foreach (GameObject x in VictoriaSecretModels) {
			x.SetActive (true);
		}
	}

	public void QuestFailed() {
		//show anxiety pop up
		//state that the player failed
		//make the cats stop and be uninteractable 
		TestPlayerController.s_instance.GetComponent<HealthHandler> ().TakeStress (25);
		TextManager.s_instance.SetNotification ("You killed the wrong cat - now they will kill you", 6f);
		foreach (WhodunitCat x in WhoDunitCats) {
			x.FailState ();
			x.GetComponentInChildren<Light> ().color = Color.red;
		}
	}

}
