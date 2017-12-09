using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineHardCut : MonoBehaviour {

	public CinemachineBrain thisBrain;
	public Cinemachine.CinemachineFreeLook thisFreelook;
	float disableTime=.5f, disableTimer;
	bool isDisabled;
	// Use this for initialization
	void Start () {
		thisBrain = FindObjectOfType<CinemachineBrain> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (isDisabled)
		{
			if (GenericTimer.RunGenericTimer (disableTime, ref disableTimer)) {
				isDisabled = false;
				thisBrain.enabled = true;
				thisFreelook.enabled = true;

			}
		}
	}
		
	public void tempDisable() {
		thisBrain.enabled = false;
		thisFreelook.enabled = false;
		isDisabled = true;
	}

	public void HardCut() {
		thisBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
	}

	public void LongTransition() {
		thisBrain.m_DefaultBlend.m_Time = 50f;
	}
}
