using UnityEngine;
using System.Collections;
using UnityEngine.AI;


public enum CatStates {Idle, Talking, Following, Waypoints, Runaway};

public class CatLogic : MonoBehaviour {

    public Animator thisCatAnimator;
	protected NavMeshAgent thisNavMeshAgent;
    protected CatStates thisCatState;
    float catRunSpeed = .02f;
    float triggerFollowDistance = 10f;
    float catLookAngle = 50;
    float waypointToggleDistance = 3f;
	float runawayTimer, runawayTime;
	//cat runs back to this point after it attacks player
	Vector3 runawayTarget;

	[SerializeField]
	protected SkinnedMeshRenderer thisSkinnedMeshRenderer;


    public GameObject[] waypoints;
    int waypointIndex;
    // Use this for initialization

	[SerializeField]
    bool switchToFollowing, switchToSitting, switchToWaypoints, switchToRunaway;
	protected void Start () {
		thisNavMeshAgent = GetComponent<NavMeshAgent> ();
		if (waypoints[0] == null) {
			waypoints = GameObject.FindGameObjectsWithTag ("waypoints");
		}
		SwitchToState (CatStates.Waypoints);
		if (thisSkinnedMeshRenderer == null)
			thisSkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer> ();
	}
	
	// Update is called once per frame
	protected void Update () {
	switch (thisCatState)
        {
            case CatStates.Idle:
                WaitForPlayerToComeClose();
                if (switchToFollowing)
                {
					SwitchToState (CatStates.Following);
                    switchToFollowing = false;
                }
                break;

		case CatStates.Talking:

			break;

		case CatStates.Following:
			FollowPlayer ();
			if (CheckIsPlayerLookingAtCat ()) {
				switchToSitting = true;
			}
			if (switchToSitting) {
				thisCatAnimator.SetTrigger ("idle");
				thisCatState = CatStates.Idle;
				switchToSitting = false;
			}
			if (switchToRunaway) {
				SwitchToState (CatStates.Runaway);
				switchToRunaway = false;
			}
                break;

		case CatStates.Waypoints:
			HandleWaypointChecking ();
                break;


		case CatStates.Runaway:
			thisNavMeshAgent.SetDestination (runawayTarget);
			CheckDistanceToRunawayTarget ();
			if (switchToFollowing) {
				SwitchToState (CatStates.Following);
				switchToFollowing = false;
			}
				
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

	void CheckDistanceToRunawayTarget()
	{
		if (Vector3.Distance(transform.position, runawayTarget) < 1f) {
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
			switchToRunaway = true;
            //DestroyCat();

        }
    }

	public void SwitchToState(CatStates switchToThisState) {
		switch (switchToThisState) {
		case CatStates.Waypoints:
			thisCatState = CatStates.Waypoints;
			thisNavMeshAgent.isStopped = false;
			GotoNextWaypoint ();
			thisCatAnimator.SetTrigger("run");
			break;

		case CatStates.Idle:
			thisCatState = CatStates.Idle;
			thisCatAnimator.SetTrigger ("idle");
			thisNavMeshAgent.isStopped = true;
			break;
		case CatStates.Following:
			thisCatAnimator.SetTrigger ("run");
			thisCatState = CatStates.Following;
			thisNavMeshAgent.isStopped = false;
			runawayTarget = transform.position;
			break;
		case CatStates.Runaway:
			thisCatAnimator.SetTrigger ("run");
			thisCatState = CatStates.Runaway;
			thisNavMeshAgent.isStopped = false;
			break;
		}
	}


    protected void DestroyCat()
    {
//        PlayerController.s_instance.ReceiveAnxiety();
        Destroy(gameObject);
    }

}
