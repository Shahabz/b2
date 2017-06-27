using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class OverlayManager : MonoBehaviour {

	public Sprite[] infectionSprites;
	public Sprite[] bloodSprites;
	public Sprite[] criminalSprites;
	public Image infection, blood;

	bool isOscillatingBloodVeil = false;
	float oscillationSpeed = 1f;
	float startingAlphaforBloodSprite, bloodSpriteFadeoutSpeed = .0005f;

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
	
	// Update is called once per frame
	void Update () {
		if (isOscillatingBloodVeil) {
			OscillateBloodVeil ();
			FadeOutBloodSprite ();
		}
	}

	public void SetImageOscillation (bool isOscillating, float speed = 1f) {
		isOscillatingBloodVeil = isOscillating;
		oscillationSpeed = speed;
	}

	public void ShowDeathOverlay() {
		int choose = Random.Range (0, infectionSprites.Length - 1);
		infection.enabled = true;
		infection.sprite = infectionSprites [choose];
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

	public void OscillateBloodVeil(){
		float materialVal = .1f + Mathf.PingPong (Time.time * 4f, .5f);
		blood.material.SetFloat ("_Cutoff", materialVal);
	}

	public void SetTransparencyOnBloodSprite(float alpha) {
		blood.enabled = true;
		startingAlphaforBloodSprite = alpha;
		Color temp = blood.material.color;
		temp.a = alpha;
		blood.material.SetColor ("_Color", temp);
	}

	void EnableRandomBloodSprite () {
		int choose = Random.Range (0, bloodSprites.Length - 1);
		blood.enabled = true;
		blood.sprite = bloodSprites [choose];
	}

	IEnumerator TurnOffThisImage(Image thisImage) {
		yield return new WaitForSeconds (5f);
		thisImage.enabled = false;
	}

	void FadeOutBloodSprite () {
		startingAlphaforBloodSprite -= bloodSpriteFadeoutSpeed;
		Color temp = blood.material.color;
		temp.a = startingAlphaforBloodSprite;
		blood.material.SetColor ("_Color", temp);
		if (startingAlphaforBloodSprite <= 0) {
			isOscillatingBloodVeil = false;
			blood.enabled = false;
		}

	}
}
