using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFogWall : MonoBehaviour {

    public Transform fogWall;
    public float minFog = 1f;
    public float maxFog = 10f;

    public float minDistance = 10f;

    Transform player;

	void Start () {
        player = TestPlayerController.s_instance.transform;
	}
	
	void Update () {
        if(fogWall) {
            if (Vector3.Distance(fogWall.position, player.position) < minDistance)
            {
                GetComponent<UnityStandardAssets.ImageEffects.GlobalFog>().height = Mathf.Lerp(minFog, maxFog, Vector3.Distance(fogWall.position, player.position) / minDistance);
            }
    //        else
    //        {
    //            GetComponent<UnityStandardAssets.ImageEffects.GlobalFog>().height = minDistance;
    //        }
        }
	}
}
