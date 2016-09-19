using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NoteToSelf {
	public string noteContents;
	public int numberOfTimesViewed; //used to determine when 
	public int importance;
	public bool isInDialogue = false;
	public string title; //for deletion sake
	public NoteToSelf(string newNoteContents, int newImportance = 5) {
		noteContents = newNoteContents;
		importance = newImportance;
	}
}

public class GUIManager : MonoBehaviour {

	//TODO implement stamina logic
	public Slider hungerSlider, anxietySlider;
	public GameObject menu;
	public Camera currentCameraInUse;
	bool isInventoryOn = true;
	[SerializeField]
	Text subtitleText, thoughtText, gameover, timeDisplay, notes, money;
	public static GUIManager s_instance;
	public bool isInDialogue;
	public List<NoteToSelf> notesToSelf = new List<NoteToSelf> ();

	bool thoughtTimerSwitch = false;
	bool dialogueTimerSwitch = false;
	float thoughtDuration, thoughtTimer;
	float dialogueDuration, dialogueTimer;

	void Awake() {
		if (s_instance == null) {
			s_instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}
	
	float subtitleTimer = 0, subtitleTimerStart;
	// Use this for initialization
	void Start () {
//		if (subtitleTimer > Time.time - subtitleTimerStart) {
//			subtitleText.enabled = true;
//		}
//		else {
//			subtitleText.enabled = false;
//		}
//			
	}
	
	// Update is called once per frame
	void Update () {
		
		BillboardCamera ();
//		money.text = "Money: $" + PlayerController.s_instance.money.ToString("F2");


		if (thoughtTimer > 0) {
			thoughtTimer -= Time.deltaTime;
		} else if (thoughtTimer <= 0 && isDisplayingThought) {
			isDisplayingThought = false;
			thoughtText.gameObject.GetComponent<Fader> ().StartFadeOut ();

		}
			
	}
	bool isDisplayingThought = false;
	public void SetThoughtText (string text, float timer = 3.5f) {
		thoughtTimer = timer;
		thoughtText.text = text;
		thoughtText.gameObject.GetComponent<Fader> ().StartFadeIn ();
		isDisplayingThought = true;

	}

	public void SetSubtitleText (string text, float timer = 3f) {
		subtitleTimer = timer;
		subtitleText.text = text;
	}

	public void AddNoteToSelf(NoteToSelf thisNote) {
		notesToSelf.Add (thisNote);
	}

	void EnableMenu () {
		Time.timeScale = 0;
//		DisplayNotesToSelf ();

	}

	void DisableMenu () {
		Time.timeScale = 1f;
	}


	void BillboardCamera () {
		Vector3 v = transform.position - currentCameraInUse.transform.position;
		v.x = v.z = 0.0f;
		thoughtText.transform.LookAt( currentCameraInUse.transform.position - v ); 
		thoughtText.transform.Rotate(0,180,0);
	}

//	void DisplayNotesToSelf () {
//		string allNotesToSelf = "Notes to Self:\n";
//		foreach (NoteToSelf x in notesToSelf) {
//			if (x.numberOfTimesViewed >= x.importance) {
//				notesToSelf.Remove (x);
//			} else {
//				x.numberOfTimesViewed++;
//				allNotesToSelf += x.noteContents + "\n";
//			}
//		}
//		notes.text = allNotesToSelf;
//	}
}
