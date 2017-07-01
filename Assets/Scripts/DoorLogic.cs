using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour, IInteractable {

    bool isOpening, isOpened;
    public float endRotation = 80f;
    public Transform rotatorRoot;
    public AudioSource doorOpenSound;
	// Use this for initialization

    private float startTime, duration = 2f;

	void Start () {
		if (gameObject.layer != LayerMask.NameToLayer("Interactable"))
		{
			Debug.LogError("Needs to be on interactable layer to interact with", this);
		}
		if (rotatorRoot == null)
        {
            rotatorRoot = transform;
        }
	}
	
	// Update is called once per frame
	void Update () {

		if (isOpening)
        {
            float t = (Time.time - startTime) / duration;
            rotatorRoot.localRotation = Quaternion.Euler(new Vector3(0f, Mathf.SmoothStep(0, endRotation, t), 0));
            if (t > 1)
            {
                isOpening = false;
            }
        }
	}

	public void Interact()
    {
		if (!isOpened) {
			OpenDoor ();
		}
    }

    void OpenDoor()
    {
        isOpening = true;
        startTime = Time.time;
        doorOpenSound.Play();
        isOpened = true;
        
    }
}

