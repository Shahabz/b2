﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		stress = maxStress;
	}
	
	void Update () {
		if(health < maxHealth && Time.time >= lastDamage + healthRegenDelay) {
			health += healthRegenAmount * Time.deltaTime;
		}

		if(stress < maxStress && Time.time >= lastStressor + stressRegenDelay) {
			stress += stressRegenAmount * Time.deltaTime;
		}
	}

	public void TakeDamage(float damageAmount) {
		lastDamage = Time.time;
		health -= damageAmount;
		if(health <= 0)
			Death();
	}

	public void TakeStress(int stressAmount) {
		lastStressor = Time.time;
		stress -= stressAmount;
	}

	void OnGUI() {
		GUI.Label(new Rect(Screen.width/2f - 50f, 0f, 100f, 30f), "Health: " + (int)health + "/" + maxHealth);
		GUI.Label(new Rect(Screen.width/2f - 50f, 30f, 100f, 30f), "Stress: " + (int)stress + "/" + maxStress);
	}

	public void Death() {
		//Do game over BS
	}
}
