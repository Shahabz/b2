using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour {

	public int maxHealth = 100;
	public int maxStress = 100;

	public float healthRegenDelay = 3f;
	public float stressRegenDelay = 3f;

	public float healthRegenAmount = 5f;
	public float stressRegenAmount = 5f;

	float lastDamage = 0f;
	float lastStressor = 0f;

	float health;
	float stress;

	public Slider anxietySlider;

	public enum StressLevel {
		None, Mild, Nervous, Shaking, FreakingOut, Insane
	}

	/// <summary>
	/// Gets the stress level as an enum of the percentage of stress.
	/// </summary>
	/// <value>The stress level.</value>
	public StressLevel stressLevel {
		get {
			return (StressLevel)Mathf.FloorToInt(stress/maxStress);
		}
	}

	void Start () {
		health = maxHealth;
		stress = 0;
	}
	
	void Update () {
		if(health < maxHealth && Time.time >= lastDamage + healthRegenDelay) {
			health += healthRegenAmount * Time.deltaTime;
		}

		/*if(stress < maxStress && Time.time >= lastStressor + stressRegenDelay) {
			stress += stressRegenAmount * Time.deltaTime;
		}*/
	}

	public void TakeDamage(float damageAmount) {
		lastDamage = Time.time;
		health -= damageAmount;
		if(health <= 0)
			Death();
	}

	public void ReduceStress (int stressAmount) {

		stress -= stressAmount;
		if (stress < 0)
			stress = 0;
		OverlayManager.s_instance.anxietySlider.value = stress / 100;
		OverlayManager.s_instance.anxietyText.text = "Anxiety: " + stress +"%";
		OverlayManager.s_instance.ShowAnxietyFadeOut (.05f, false);

	}

	public void TakeStress(int stressAmount) {
		if (stress >= 100) {
			return;
		}
		lastStressor = Time.time;
		TestPlayerController.s_instance.PlayBark ();
		stress += stressAmount;
		OverlayManager.s_instance.anxietySlider.value = stress / 100;
		OverlayManager.s_instance.anxietyText.text = "Anxiety: " + stress +"%";
		if (stress >= 100) {
			Death ();
		}
		else if (stressAmount >= 40) {
			OverlayManager.s_instance.ShowAnxietyFadeOut (.1f);
			SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.AnxietyHit3);

		} else if (stressAmount >= 10) { 
			OverlayManager.s_instance.ShowAnxietyFadeOut (.07f);
			if (SoundtrackManager.s_instance!=null)SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.AnxietyHit4);

		} else if (stressAmount >= 0) {
			OverlayManager.s_instance.ShowAnxietyFadeOut (.05f);
			SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.AnxietyHit5);
		} 
	}

	/*void OnGUI() {
		GUI.Label(new Rect(Screen.width/2f - 50f, 0f, 100f, 30f), "Health: " + (int)health + "/" + maxHealth);
		GUI.Label(new Rect(Screen.width/2f - 50f, 30f, 100f, 30f), "Stress: " + (int)stress + "/" + maxStress);
	}*/

	public void Death() {
		//Do game over BS
		//GameObject.Find("DeathCam").GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = true;
		OverlayManager.s_instance.ShowDeathOverlay();
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.deathguillo);
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.deathscream);
		TestPlayerController.s_instance.lockInput= TestPlayerController.InputLock.Locked;
		TestPlayerController.s_instance.GetComponent<Animator> ().SetTrigger ("death");
		TestUIManager.instance.SetState (TestUIManager.UIState.Cutscene);
		StartCoroutine ("Restart");
	}

	IEnumerator Restart()
	{
		yield return new WaitForSeconds (4f);
		TextManager.s_instance.SetPrompt ("Press 'F' to restart level");
		TestPlayerController.s_instance.SetPlayerMode (PlayerMode.Death);
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.songoflord);
	}
}
