using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

	[System.Serializable]
	public class Weapon {
		public string name;
		public AudioClip[] fireSounds;
		public AudioClip[] emptySounds;
		public AudioClip[] reloadSounds;
		public float fireRate = 0.5f;
		public float recoil = 5f;
		public int maxAmmo = 0;
		[HideInInspector]
		public int currentAmmo = 0;
		public Transform weaponObj;
	}

	public Weapon[] weapons;
	int currentWeaponIndex;
	public Weapon CurrentWeapon {
		get {
			return weapons[currentWeaponIndex];
		}
	}

	const float MAX_DISTANCE = 50f; //Fire distance. Magic number unless I need to change it

	float fireCD = 0f;

	void Start() {
		for(int i = 0; i < weapons.Length; i++) {
			weapons[i].currentAmmo = weapons[i].maxAmmo;
		}
	}

	void Update() {
		if(fireCD > 0f)
			fireCD -= Time.deltaTime;
	}

	public bool CanFire() {
		if(CurrentWeapon.currentAmmo <= 0) {
			GetComponent<AudioSource>().volume = OptionManager.FXVolume;
			GetComponent<AudioSource>().clip = CurrentWeapon.emptySounds[Random.Range(0, CurrentWeapon.emptySounds.Length)];
			GetComponent<AudioSource>().Play();
			return false;
		}
		return (fireCD <= 0f);
	}

	public void Fire() {
		CurrentWeapon.currentAmmo -= 1;
		fireCD = CurrentWeapon.fireRate;
		//Play sound here
		GetComponent<AudioSource>().volume = OptionManager.FXVolume;
		GetComponent<AudioSource>().clip = CurrentWeapon.fireSounds[Random.Range(0, CurrentWeapon.fireSounds.Length)];
		GetComponent<AudioSource>().Play();

		ParticleSystem muzzleFlash = CurrentWeapon.weaponObj.Find("MuzzleFlash") != null ? CurrentWeapon.weaponObj.Find("MuzzleFlash").GetComponent<ParticleSystem>() : null;
		if(muzzleFlash != null) {
			muzzleFlash.Emit(30);//(true);
		}

		Transform firePos = CurrentWeapon.weaponObj.Find("FirePos");
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
		CurrentWeapon.currentAmmo = CurrentWeapon.maxAmmo;
		GetComponent<AudioSource>().volume = OptionManager.FXVolume;
		GetComponent<AudioSource>().clip = CurrentWeapon.reloadSounds[Random.Range(0, CurrentWeapon.reloadSounds.Length)];
		GetComponent<AudioSource>().Play();
	}

	public void Melee() {
//		GetComponent<
	}

//	public void SwitchWeapon() {
//
//	}
}
