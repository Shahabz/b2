using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MultipleChoice : MonoBehaviour {
	
	protected int selection = 0;
	public GameObject[] currentSelectionIndicators;

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
				currentSelectionIndicators [i].GetComponent<Text>().enabled = true;
			} else {
				currentSelectionIndicators [i].GetComponent<Text>().enabled = false;
			}
		}
	}

	public virtual void SelectItem(){}

	

}
