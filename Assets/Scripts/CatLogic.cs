using UnityEngine;
using System.Collections;

enum CatStates {Sitting, Following};

public class CatLogic : MonoBehaviour {

    public Animator thisCatAnimator;
    
    CatStates thisCatState;
    float catRunSpeed = .02f;
    float triggerFollowDistance = 10f;
    float catLookAngle = 50;
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
                if (CheckIsPlayerLookingAtCat()) { switchToSitting = true; }
                if (switchToSitting)
                {
                    thisCatAnimator.SetTrigger("idle");
                    thisCatState = CatStates.Sitting;
                    switchToSitting = false;
                }
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
            if (!CheckIsPlayerLookingAtCat())
            {
                switchToFollowing = true;
            }
        }
    }

    bool CheckIsPlayerLookingAtCat()
    {
        if (GetAngleBetweenPlayerForwardAndCat() < catLookAngle)
        {
           return true;
        }
        else { return false; }
    }

    float GetAngleBetweenPlayerForwardAndCat() {
        Vector3 playerForward = PlayerController.s_instance.transform.forward;
        Vector3 towardCat = -PlayerController.s_instance.transform.position + transform.position;
        return Vector3.Angle(playerForward, towardCat);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController.s_instance.switchToPassiveState = true;
            DestroyCat();

        }
    }

    void DestroyCat()
    {
        PlayerController.s_instance.ReceiveAnxiety();
        Destroy(gameObject);
    }

}
