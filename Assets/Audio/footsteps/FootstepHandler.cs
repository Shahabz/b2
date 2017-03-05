using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepHandler : MonoBehaviour {

    public GameObject[] footSteps;
    GameObject currentFootstep = null;
    int upperMax;
    float cooldown = .5f, fadeSpeed = 0.15f;
    public float walkCooldown = .5f, runCooldown = .2f;

    // Use this for initialization
    void Start () {
        footSteps = GameObject.FindGameObjectsWithTag("footstep");
        StartCoroutine("DestroyAllFootStepIdleObjs");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayFootStep(float moveMagnitude)
    {
        if (moveMagnitude < .9f)
        {
            cooldown = walkCooldown;
        }
        else
        {
            cooldown = runCooldown;
        }
        if (currentFootstep == null)
        {
            upperMax = footSteps.Length;
            int footStepIndex = UnityEngine.Random.Range(0, upperMax);
            currentFootstep = (GameObject)Instantiate(footSteps[footStepIndex]);
            currentFootstep.tag = "foot";
            currentFootstep.GetComponent<AudioSource>().volume = OptionManager.FXVolume;
            currentFootstep.GetComponent<AudioSource>().Play();
            //a timer could work starttime = Time.time, a counter is always counting in update, a bool is set for footstep playing, and it goes off after timer catches up to timeTilFootstep can play
            //I use coroutine instead
            StartCoroutine("NullifyFootstep");
        }
    }

    IEnumerator NullifyFootstep()
    {
        yield return new WaitForSeconds(cooldown);
        currentFootstep = null;
    }
    public void CallCeaseFootStep()
    {
        StartCoroutine("CeaseFootstep");

    }
    IEnumerator CeaseFootstep()
    {
        while (currentFootstep != null && currentFootstep.GetComponent<AudioSource>().volume > 0.0f)
        {                   //where x is sound track file
            currentFootstep.GetComponent<AudioSource>().volume -= 0.1f;
            yield return new WaitForSeconds(fadeSpeed);
        }
    }

    IEnumerator DestroyAllFootStepIdleObjs()
    {
        while (true)
        {
            yield return new WaitForSeconds(4);
            GameObject[] footsteps = GameObject.FindGameObjectsWithTag("foot");
            foreach (GameObject f in footsteps)
                if (f.GetComponent<AudioSource>().isPlaying == false)
                    Destroy(f);

        }
    }
}
