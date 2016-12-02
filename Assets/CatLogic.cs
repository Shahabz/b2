using UnityEngine;
using System.Collections;

enum CatStates {Sitting, Following};

public class CatLogic : MonoBehaviour {

    public Animator thisCatAnimator;
    CatStates thisCatState;
    float catRunSpeed = 5f;
    float triggerFollowDistance = 10f;
    // Use this for initialization

    bool switchToFollowing, switchToSitting;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	switch (thisCatState)
        {
            case CatStates.Sitting:
                WaitForPlayerToComeClose();
                if (switchToFollowing)
                {
                    thisCatAnimator.SetTrigger("run");
                    thisCatState = CatStates.Following;
                    switchToFollowing = false;
                }
                break;

            case CatStates.Following:
                FollowPlayer();
                break;
        }
	}

    void FollowPlayer()
    {
        transform.LookAt(PlayerController.s_instance.transform.position);
        transform.Translate(Vector3.forward * catRunSpeed);
        
    }

    void WaitForPlayerToComeClose()
    {
        if (Vector3.Distance(transform.position, PlayerController.s_instance.transform.position) < triggerFollowDistance) {
            switchToFollowing = true;
        }
    }
}
