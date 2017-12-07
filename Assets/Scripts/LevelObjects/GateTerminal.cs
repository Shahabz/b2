﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTerminal : MonoBehaviour, IInteractable {

	public GameObject linkedGate;
	public GameObject[] terminalIndicators;
	int terminalsDestroyed;
	bool hasEnabledGate;
	public Material greenSelectedMaterial;
	public GameObject TheFedIsComing;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Interact () {
		if (terminalsDestroyed == terminalIndicators.Length) {
			EnableGate ();
		} else {
			TextManager.s_instance.SetNotification ("Quarantine Gate Locked \n" + (terminalIndicators.Length - terminalsDestroyed).ToString() + " Terminals Remain");
		}
	}

	public void BlueTerminalDestroyed() {
		terminalIndicators [terminalsDestroyed].GetComponent<MeshRenderer> ().material = greenSelectedMaterial;
		terminalsDestroyed++;
		TextManager.s_instance.SetNotification(terminalsDestroyed.ToString() + "/" + terminalIndicators.Length.ToString() + " Terminals Destroyed", 2f);
		if (terminalsDestroyed == terminalIndicators.Length) {
			TheFedIsComing.SetActive (true);
		}
	}

	void EnableGate () {
		if (!hasEnabledGate) {
			hasEnabledGate = true;
			TextManager.s_instance.SetNotification ("Quarantine Gate Unlocked");
			linkedGate.GetComponent<Elevator> ().isActive = true;
		}
	}
}
