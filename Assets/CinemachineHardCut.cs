using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineHardCut : MonoBehaviour {

	CinemachineBrain thisBrain;

	// Use this for initialization
	void Start () {
		thisBrain = FindObjectOfType<CinemachineBrain> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HardCut() {
		thisBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
	}
}
