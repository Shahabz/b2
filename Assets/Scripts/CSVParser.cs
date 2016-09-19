using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSVParser : MonoBehaviour {

	public List<string> listOfStrings = new List<string>();
	
	public List<string> Parse(TextAsset csvString){

		//take each line, split by comma, and then populate list at that index
		string[] arrayOfStrings = csvString.ToString().Split(","[0]);
		for (int j=0; j<arrayOfStrings.Length; j++){
			string temp = arrayOfStrings[j].Replace('|',',');

			listOfStrings.Add (temp);
		}
		return listOfStrings;
	}
}
