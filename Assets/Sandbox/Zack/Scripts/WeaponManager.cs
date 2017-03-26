﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

	[System.Serializable]
	public class Weapon {
		public string name;
		public AudioClip[] fireSounds;
		public float fireRate = 0.5f;
		public float recoil = 5f;
	}

	public Weapon[] weapons;
	int currentWeaponIndex;
	public Weapon CurrentWeapon {
		get {
			return weapons[currentWeaponIndex];
		}
	}

	public Transform weaponObj;

	const float MAX_DISTANCE = 50f;

	float fireCD = 0f;

	void Update() {
		if(fireCD > 0f)
			fireCD -= Time.deltaTime;
	}

	public bool CanFire() {
		return (fireCD <= 0f);
	}

	public void Fire() {
		fireCD = CurrentWeapon.fireRate;
		//Play sound here
		GetComponent<AudioSource>().volume = OptionManager.FXVolume;
		GetComponent<AudioSource>().clip = CurrentWeapon.fireSounds[Random.Range(0, CurrentWeapon.fireSounds.Length)];
		GetComponent<AudioSource>().Play();

		ParticleSystem muzzleFlash = weaponObj.FindChild("MuzzleFlash") != null ? weaponObj.FindChild("MuzzleFlash").GetComponent<ParticleSystem>() : null;
		if(muzzleFlash != null) {
			muzzleFlash.Emit(30);//(true);
		}

		Transform firePos = weaponObj.FindChild("FirePos");
		RaycastHit hit;
		LayerMask hitMask = LayerMask.GetMask(new string[] {"Default"});
		if(Physics.Raycast(new Ray(firePos.position, firePos.up), out hit, MAX_DISTANCE, hitMask, QueryTriggerInteraction.Ignore)) {
			Debug.DrawRay(firePos.position, firePos.up*MAX_DISTANCE, Color.red, 1f);

			RagdollCharacter ragdoll = hit.collider.gameObject.GetComponentInParent<RagdollCharacter>();
			if(ragdoll != null)
				ragdoll.Activate(hit.point, firePos.up*3000f);

//			IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();
//			if((damageable = hit.collider.gameObject.GetComponent<IDamageable>()) != null) {
//				damageable.ApplyDamage(hit.point);
//			} else if((damageable = hit.collider.gameObject.GetComponentInParent<IDamageable>()) != null) {
//				damageable.ApplyDamage(hit.point);
//			} else {
//				//Just do some decal shit I guess. Rather seperate it for quick spin up and not having to maintain complexity
//			}

		}
	}

	public void Reload() {

	}

	public void Melee() {
//		GetComponent<
	}

//	public void SwitchWeapon() {
//
//	}
}
