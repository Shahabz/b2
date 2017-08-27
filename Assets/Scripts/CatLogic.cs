using UnityEngine;
using System.Collections;
using UnityEngine.AI;


public enum CatStates {Idle, Talking, Following, Waypoints};

public class CatLogic : MonoBehaviour {

    public Animator thisCatAnimator;
	protected NavMeshAgent thisNavMeshAgent;
    protected CatStates thisCatState;
    float catRunSpeed = .02f;
    float triggerFollowDistance = 10f;
    float catLookAngle = 50;
    float waypointToggleDistance = 3f;


    public GameObject[] waypoints;
    int waypointIndex;
    // Use this for initialization

	[SerializeField]
    bool switchToFollowing, switchToSitting, switchToWaypoints;
	protected void Start () {
		thisNavMeshAgent = GetComponent<NavMeshAgent> ();
		SwitchToState (CatStates.Waypoints);
		thisCatAnimator.SetTrigger("run");

	}
	
	// Update is called once per frame
	protected void Update () {
	switch (thisCatState)
        {
            case CatStates.Idle:
                WaitForPlayerToComeClose();
                if (switchToFollowing)
                {
                    thisCatAnimator.SetTrigger("run");
                    thisCatState = CatStates.Following;
                    switchToFollowing = false;
                }
                break;

		case CatStates.Talking:

			break;

            case CatStates.Following:
                FollowPlayer();
                if (CheckIsPlayerLookingAtCat()) { switchToSitting = true; }
                if (switchToSitting)
                {
                    thisCatAnimator.SetTrigger("idle");
                    thisCatState = CatStates.Idle;
                    switchToSitting = false;
                }
                break;

		case CatStates.Waypoints:
			HandleWaypointChecking ();
                break;

        }
	}

    void FollowPlayer()
    {
		transform.LookAt(TestPlayerController.s_instance.transform.position);
        transform.Translate(Vector3.forward * catRunSpeed);
        
    }

    void GotoNextWaypoint()
    {
       
        //transform.LookAt(waypoints[waypointIndex].transform);
		thisNavMeshAgent.SetDestination (waypoints [waypointIndex].transform.position);

    }

    void HandleWaypointChecking()
    {
        if (Vector3.Distance(transform.position, waypoints[waypointIndex].transform.position) < waypointToggleDistance) {
            if (waypointIndex >= waypoints.Length - 1)
            {
                waypointIndex = 0;
            }
            else
            {
                waypointIndex++;
            }
        }
		GotoNextWaypoint ();

    }

    void WaitForPlayerToComeClose()
    {
		if (Vector3.Distance(transform.position, TestPlayerController.s_instance.transform.position) < triggerFollowDistance) {
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
		Vector3 playerForward = TestPlayerController.s_instance.transform.forward;
		Vector3 towardCat = -TestPlayerController.s_instance.transform.position + transform.position;
        return Vector3.Angle(playerForward, towardCat);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
//            PlayerController.s_instance.switchToPassiveState = true;
            //DestroyCat();

        }
    }

	protected void SwitchToState(CatStates switchToThisState) {
		switch (switchToThisState) {
		case CatStates.Waypoints:
			thisCatState = CatStates.Waypoints;
			GotoNextWaypoint ();
			break;

		case CatStates.Idle:
			thisCatState = CatStates.Idle;

			break;
		}

	}


    void DestroyCat()
    {
//        PlayerController.s_instance.ReceiveAnxiety();
        Destroy(gameObject);
    }

}
