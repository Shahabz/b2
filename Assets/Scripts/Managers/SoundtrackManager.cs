using UnityEngine;
using System.Collections;

public class SoundtrackManager : MonoBehaviour {

	public AudioSource
	AnxietyHit1,
	AnxietyHit2,
	AnxietyHit3,
	AnxietyHit4,
	AnxietyHit5,
	Fed_AnxietyLightning,
	Player_HeartBeatFast,
	Player_HeartBeatSlow,
	uruhit2,
	deathguillo,
	deathscream,
	gunshot,
	songoflord,
	xanax,
	xanax2,
	xanax3,
	xanax4,
	xanax5,
	catkill1,
	catkill2,
	catkill3,
	catkill4,
	accessgranted,
	accessdenied

	; //soundtrack files


	public static SoundtrackManager s_instance;

	void Awake () {
		s_instance = this; 
		//DontDestroyOnLoad (gameObject); //persist through scenes
	}

	public void FadeOut(AudioSource y)
	{
		StartCoroutine (FadeOutAudioSource (y));
	}

	IEnumerator FadeOutAudioSource(AudioSource x) { //call from elsewhere
		while (x.volume > 0.0f) {					//where x is sound track file
			x.volume -= 0.01f;
			yield return new WaitForSeconds(0.03f);
		}
		x.Stop ();
	}

	public void PlayAudioSource(AudioSource x) { //call from elsewhere
		x.volume = 1;
		x.Play ();
	}
}
