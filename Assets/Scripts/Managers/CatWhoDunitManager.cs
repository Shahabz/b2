using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWhoDunitManager : MonoBehaviour {

	public bool hasDisplayedPuzzlePrompt;
	int catsCompleted = 0; //must complete five before win state can be activated, prevents random luck from succeeding
	GameObject thisCatTriggeredDisplayPrompt;
	string puzzlePrompt = "Help. None of us are actually cats, we are all Victoria Secret Models, and we are quadruplets - like twins but with the number four.\n\nWe were on a flight to Croatia when of a sudden our plane ran into a hurricane and spiraled out of control. We crash landed nearby. The pilot of\n\nthe airplane was the only other survivor - and he tried to rape us. We fought him off, and when we finally defeated him he became very upset.\n\nEverything went black, and we heard the words 'If I cannot have you, no one can.' And then like that, we all turned into cats. He put a curse on us and he is still amongst us right out. One of us is the pilot. It could be me. The only way to turn back us into humans is to kill him, to kill the rapist pilot. However, if you kill the wrong one, it will perpetuate the curse until the end of time.";
	public WhodunitCat[] WhoDunitCats;
	public GameObject[] VictoriaSecretModels;

	// Use this for initialization
	void Start () {
		WhoDunitCats = GameObject.FindObjectsOfType<WhodunitCat> ();
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
		hasDisplayedPuzzlePrompt = true;
	}

	public void OnFinishPrompt() {
		thisCatTriggeredDisplayPrompt.GetComponent<WhodunitCat> ().SwitchToState (CatStates.Waypoints);
	}

	public void QuestSucceeded() {
		//pop up all of the models of women and kill the man
		//the women just stand their, and if you walk into them, they turn into skeletons
		TextManager.s_instance.SetPrompt ("You Saved the Models");
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
		TextManager.s_instance.SetPrompt ("You Killed the Wrong Cat");
		foreach (WhodunitCat x in WhoDunitCats) {
			x.FailState ();
		}
	}

}
