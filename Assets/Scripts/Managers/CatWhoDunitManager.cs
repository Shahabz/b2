using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatWhoDunitManager : MonoBehaviour {

	public bool hasDisplayedPuzzlePrompt;
	int catsCompleted = 0; //must complete five before win state can be activated, prevents random luck from succeeding

	string puzzlePrompt = "Help. None of us are actually cats, we are all Victoria Secret Models, and we are quadruplets - like twins but with the number four.\n\nWe were on a flight to Croatia when of a sudden our plane ran into a hurricane and spiraled out of control. We crash landed nearby. The pilot of\n\nthe airplane was the only other survivor - and he tried to rape us. We fought him off, and when we finally defeated him he became very upset.\n\nEverything went black, and we heard the words 'If I cannot have you, no one can.' And then like that, we all turned into cats. He put a curse on us and he is still amongst us right out. One of us is the pilot. It could be me. The only way to turn back us into humans is to kill him, to kill the rapist pilot. However, if you kill the wrong one, it will perpetuate the curse until the end of time.";



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CatCompleted () {
		catsCompleted++;
	}

	public void DisplayPrompt() {
		GetComponent<DialogueSystem> ().StartDialogue ();
		hasDisplayedPuzzlePrompt = true;
	}

}
