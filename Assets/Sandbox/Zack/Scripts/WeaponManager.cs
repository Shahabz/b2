using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

	public Transform weaponObj;

	const float MAX_DISTANCE = 50f;

	public void Fire() {
		//Play sound here
//		GetComponent<AudioSource>().clip = weapons[currentWeapon].fireSound;
//		GetComponent<AudioSource>().Play();

		Transform firePos = weaponObj.FindChild("FirePos");
		RaycastHit hit;
		LayerMask hitMask = LayerMask.GetMask(new string[] {"Default"});
		if(Physics.Raycast(new Ray(firePos.position, firePos.up), out hit, MAX_DISTANCE, hitMask, QueryTriggerInteraction.Ignore)) {
			Debug.DrawRay(firePos.position, firePos.up*MAX_DISTANCE, Color.red, 1f);

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

	}

//	public void SwitchWeapon() {
//
//	}
}
