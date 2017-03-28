using UnityEngine;
using System.Collections;

public class toggleLight : MonoBehaviour 
{
	public GameObject playerLight;
	public bool lightOn = false;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Return) && lightOn == false)
		{
			lightOn = true;
			playerLight.SetActive(true);
		}

		else if(lightOn && Input.GetKeyDown(KeyCode.Return))
		{
			lightOn = false;
			playerLight.SetActive(false);
		}
	}
}
