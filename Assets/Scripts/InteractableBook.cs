using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBook : MonoBehaviour, IInteractable {

	public GameObject[] pages;
	int pageIterator;
	bool isTurningPage;
	bool turningPagesForward = true;
	float lerpTimer, lerpTime = 2f;
	float rotatePageAmount = 160f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isTurningPage) {
			HandlePageTurn();
		}
	}

	public void Interact() {
		if (!isTurningPage) {
			lerpTimer = 0;
			isTurningPage = true;
		}
	}

	void HandlePageTurn() {
		lerpTimer += Time.deltaTime;

		if (lerpTimer / lerpTime >= 1) {
			OnPageTurnComplete ();
			lerpTimer = lerpTime;
			return;
		}
		if (turningPagesForward) {
			float angle = Mathf.Lerp (0, rotatePageAmount, lerpTimer / lerpTime);
			pages [pageIterator].transform.localRotation = Quaternion.Euler (angle, 0, 0);// (transform.position, transform.right, angle);
		} else {
			float angle = Mathf.Lerp (rotatePageAmount, 0, lerpTimer / lerpTime);
			pages [pageIterator].transform.localRotation = Quaternion.Euler (angle, 0, 0);
		}
	}

	void OnPageTurnComplete () {
		isTurningPage = false;
		if (turningPagesForward) {
			if (pageIterator == pages.Length - 1) {
				turningPagesForward = false;
			} else {
				pageIterator++;
			}
		} else {
			if (pageIterator == 0) {
				turningPagesForward = true;
			} else {
				pageIterator--;
			}
		}
	}
}
