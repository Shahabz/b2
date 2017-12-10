using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour {

	public void LoadLevelByInt(int whichlevel) 
	{
		Application.LoadLevel (whichlevel);
	}
}
