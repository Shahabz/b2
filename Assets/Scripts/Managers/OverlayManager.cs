using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OverlayManager : MonoBehaviour {

	public Sprite[] infectionSprites;
	public Sprite[] bloodSprites;
	public Sprite[] healSprites;
	public Image infection, blood;

	bool isFading = false;
	bool showDeathFX;
	float oscillationSpeed = 1f;
	float startingAlphaforBloodSprite, bloodSpriteFadeoutSpeed = .0005f;
	Fader[] AnxietyFaders;
	public Slider anxietySlider;
	public Text anxietyText, deathText;
	bool isHealing;
	public GameObject GradeBG, LOGO;
	public Text rating;
	public GameObject blackout;

	// Use this for initialization

	public static OverlayManager s_instance;

	void Awake()
	{
		if (s_instance == null)
		{
			s_instance = this;
		}
		else
		{
			Destroy(this);
		}
	}

	void Start () {
		AnxietyFaders = GetComponentsInChildren<Fader> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (showDeathFX) {
			PlayDeathPulseOverlay ();
		}
		else if (isFading) {
			FadeOutBloodSprite ();
		}

		//timeElapsed += Time.deltaTime;
		//textShownOnScreen = GetWords(fullText, timeElapsed * wordsPerSecond);


	}

	public void ShowRating() {
		GradeBG.SetActive (true);
		string ending = "Episode Complete\nGrade: ";
		int score = 0;
		if (GameManager.s_instance.saveWomen)
			score++;
		if (GameManager.s_instance.completeTherapy)
			score++;
		if (score == 0) {
			ending += "C";
		} else if (score == 1) {
			ending += "B";
		} else {
			ending += "A";
		}
		rating.text = ending;
	}
		
	public void ShowDeathOverlay() {
		showDeathFX = true; 
		deathText.gameObject.SetActive (true);
		//int choose = Random.Range (0, infectionSprites.Length - 1);
		blood.enabled = true;
		isFading = false;
		Color temp = blood.material.color;
		temp.a = 1;
		blood.material.color = temp;
	}

	public void FlashInfectionSprite() {
		int choose = Random.Range (0, infectionSprites.Length - 1);
		infection.enabled = true;
		infection.sprite = infectionSprites [choose];
		StartCoroutine ("TurnOffThisImage", infection);
	}
	public void HideBloodSprite() {
		StartCoroutine ("TurnOffThisImage", blood);

	}

	public void PlayDeathPulseOverlay(){
		float materialVal = .3f + Mathf.PingPong (Time.time/2, .5f);
		blood.material.SetFloat ("_Cutoff", materialVal);
	}

	public void ShowAnxietyFadeOut(float transparency, bool red=true) {
		if (red) {
			blood.sprite = bloodSprites [0];
		} else {
			blood.sprite = healSprites [0];
		}
		isFading = true;
		blood.enabled = true;
		startingAlphaforBloodSprite = transparency;
		Color temp = red ? Color.red : Color.green;
		temp.a = transparency;
		blood.material.SetColor ("_Color", temp);
		blood.material.SetFloat ("_Cutoff", .9f);
		foreach (Fader x in AnxietyFaders) {
			x.StartFadeOut (3);
		}

	}

	void FadeOutBloodSprite () {
		startingAlphaforBloodSprite -= bloodSpriteFadeoutSpeed;
		Color temp = blood.material.color;
		temp.a = startingAlphaforBloodSprite;
		blood.material.SetColor ("_Color", temp);
		if (startingAlphaforBloodSprite <= 0) {
			isFading = false;
			blood.enabled = false;
		}

	}


}
