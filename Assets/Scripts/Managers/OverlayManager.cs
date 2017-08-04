using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OverlayManager : MonoBehaviour {

	public Sprite[] infectionSprites;
	public Sprite[] bloodSprites;
	public Sprite[] criminalSprites;
	public Image infection, blood;

	bool isFading = false;
	bool showDeathFX;
	float oscillationSpeed = 1f;
	float startingAlphaforBloodSprite, bloodSpriteFadeoutSpeed = .0005f;
	Fader[] AnxietyFaders;
	public Slider anxietySlider;
	public Text anxietyText, deathText;


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

	public void ShowAnxietyFadeOut(float transparency) {
		isFading = true;
		blood.material.SetFloat ("_Cutoff", .9f);
		blood.enabled = true;
		startingAlphaforBloodSprite = transparency;
		Color temp = blood.material.color;
		temp.a = transparency;
		blood.material.SetColor ("_Color", temp);
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
