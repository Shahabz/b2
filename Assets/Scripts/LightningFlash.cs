using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningFlash : MonoBehaviour {

	[SerializeField]
	Light thisLight;

	// Use this for initialization
	void OnEnable () {
		StartCoroutine ("FlashLightning");
	}

	public bool bFlashTwoTimes = true;

	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator FlashLightning () {
		thisLight.enabled = true;
		yield return new WaitForSeconds (.1f);
		thisLight.enabled = false;
		if (bFlashTwoTimes) {
			yield return new WaitForSeconds (.1f);
			thisLight.enabled = true;

			yield return new WaitForSeconds (.1f);
			thisLight.enabled = false;
		}

	}
}
