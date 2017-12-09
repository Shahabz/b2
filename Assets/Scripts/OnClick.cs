using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class OnClick : MonoBehaviour {

	public UnityEvent EventToPlayOnClick;
	bool hasPlayed;
	// Update is called once per frame
	void Update () {
		if (NPInputManager.input.Fire.WasPressed) {
			if (!hasPlayed) {
				EventToPlayOnClick.Invoke ();
				hasPlayed = true;
			}
		}
	}
}
