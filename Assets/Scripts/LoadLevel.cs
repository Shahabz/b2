using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

	public Slider thisProgressSlider;
	public Text thisProgressText;
	public void LoadLevelByInt(int thislevel)
	{
		StartCoroutine(LoadLevelAsync(thislevel));
	}

	IEnumerator LoadLevelAsync (int whichlevel)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync (whichlevel);
		while (!asyncLoad.isDone) {
			if (thisProgressSlider != null)
				thisProgressSlider.value = asyncLoad.progress;
			if (thisProgressText != null)
				thisProgressText.text = (asyncLoad.progress*100).ToString("F0") + "%";
			yield return null;
		}
	}
}
