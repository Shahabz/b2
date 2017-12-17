using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XanaxPickup : MonoBehaviour, IInteractable {
	bool hasBeenPickedUp;
	public XanaxWaypoint owner;


	public void Interact () {
		if (!hasBeenPickedUp) {
			SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.xanax);
			GetComponent<BoxCollider> ().enabled = false;
			TestPlayerController.s_instance.GrabAndSwallowPills (gameObject);
			hasBeenPickedUp = true;
			if (owner) {
				owner.hasBeenAccessed = true;
			}
			StartCoroutine ("XanaxSounds");
		}
	}

	IEnumerator XanaxSounds(){
		yield return new WaitForSeconds (.4f);
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.xanax2);
		yield return new WaitForSeconds (1.1f);
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.xanax3);
		yield return new WaitForSeconds (1.1f);
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.xanax4);

		
	}
}
