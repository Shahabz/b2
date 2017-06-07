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

	public void SetImageOscillation (bool isOscillating, float speed = 1f) {
		isOscillatingBloodVeil = isOscillating;
		oscillationSpeed = speed;
	}
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
		}
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
	public void FlashBloodSprite() {
		int choose = Random.Range (0, infectionSprites.Length - 1);
		blood.enabled = true;
		blood.sprite = bloodSprites [choose];
		StartCoroutine ("TurnOffThisImage", blood);

	}

	public void OscillateBloodVeil(){
		float materialVal = Mathf.PingPong (Time.time * 2f, .8f);
		blood.material.SetFloat ("_Cutoff", materialVal);
	}

	public void SetTransparencyOnBloodSprite(float alpha) {
		blood.material.SetFloat ("_Color", alpha);
	}

	IEnumerator TurnOffThisImage(Image thisImage) {
		yield return new WaitForSeconds (.09f);
		thisImage.enabled = false;
	}
}
