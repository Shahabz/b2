using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CodeThoughts : MonoBehaviour {
	public TextAsset codeJargonThoughts, codePsychoThoughts;
	public GameObject ThoughtText;
	// Use this for initialization
	List<string> myCodeThoughts;
	CSVParser thisParser;
	bool isSpawning;
	float spawnTime = .03f, spawnTimer;
	float spawnRangeX = 1.2f;
	float spawnRangeY = .9f;
	float spawnRangeZ = 0f;
    bool isPsychoMode;
	void Start () {
		thisParser = GetComponent<CSVParser> ();
}

	public void StartSpawning(bool isPsycho) {
        isPsychoMode = isPsycho;

        if (isPsycho)
        {
            myCodeThoughts = new List<string>(thisParser.Parse(codePsychoThoughts));
        }
        else {
            myCodeThoughts = new List<string>(thisParser.Parse(codeJargonThoughts));

        }
        isSpawning = true;	
	}

	public void StopSpawning() {
		isSpawning = false;

	}

	// Update is called once per frame
	void Update ()
	{
		if (isSpawning)
		{
			spawnTimer += Time.deltaTime;
			if (spawnTimer > spawnTime)
			{
				spawnTimer = 0;
				SpawnRandomCodeThought();
			}
		}
	}



	void SpawnRandomCodeThought () {
		float x = Random.Range (-spawnRangeX, spawnRangeX);
		float y = Random.Range (-spawnRangeY, spawnRangeY);
		float z = Random.Range (-spawnRangeZ, spawnRangeZ);

		Vector3 spawnPos = new Vector3 (transform.position.x + x, transform.position.y + y, transform.position.z + spawnRangeZ);
		GameObject thoughtText = (GameObject)Instantiate (ThoughtText, spawnPos, transform.rotation) as GameObject;
		thoughtText.GetComponentInChildren<Text> ().text = myCodeThoughts [Random.Range (0, myCodeThoughts.Count - 1)];

        if (isPsychoMode)
        {
            thoughtText.GetComponentInChildren<Text>().color = Color.red;
            thoughtText.GetComponentInChildren<Fader>().customStartColor = Color.red;
            thoughtText.GetComponentInChildren<Fader>().customEndColor = new Vector4 (0,0,0,0);
        }
    }
}
