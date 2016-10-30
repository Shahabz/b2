using UnityEngine;
using System.Collections;

public class MultipleChoice : MonoBehaviour {
	
	protected int selection = 0;
	public GameObject[] currentSelectionIndicators;
	protected Vector3 lastInLevelCamPosition;
	protected Quaternion lastInLevelCamRotation;
	protected bool isActive = true;

	[SerializeField]
	protected Camera mainViewOfMultipleChoice;

	public void ArrowDown() {
		if (isActive) {
			if (selection >= currentSelectionIndicators.Length - 1) {
				selection = 0;
			} else {
				selection++;
			}
			ShowSelection ();
		}
	}

	public void ArrowUp() {
		if (isActive) {
			if (selection < 1) {
				selection = currentSelectionIndicators.Length - 1;
			} else {
				selection--;
			}
			ShowSelection ();
		}

	}
	public void ShowSelection() {
		for (int i = 0; i < currentSelectionIndicators.Length;i++) {
			if (selection == i) {
				currentSelectionIndicators [i].SetActive(true);
			} else {
				currentSelectionIndicators [i].SetActive(false);
			}
		}
	}

	public virtual void SelectItem(){}

	public void MultipleChoiceCameraOn () {
		Camera.main.transform.rotation = mainViewOfMultipleChoice.transform.rotation;
		Camera.main.transform.position = mainViewOfMultipleChoice.transform.position;
	}

	// Stores the camera position of where you were when you entered the therapy
	public void MultipleChoiceCameraOff() {
		Camera.main.transform.rotation = lastInLevelCamRotation;
		Camera.main.transform.position = lastInLevelCamPosition;
	}

	public void SetCamera(Camera thisCamera) {
		Camera.main.transform.rotation = thisCamera.transform.rotation;
		Camera.main.transform.position = thisCamera.transform.position;
		Camera.main.fieldOfView = thisCamera.fieldOfView;

	}

}
