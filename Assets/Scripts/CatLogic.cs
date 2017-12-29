using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;

public enum CatStates {Idle, Talking, Following, Waypoints, Runaway, AttackPlayer};

public class CatLogic : MonoBehaviour, IInteractable {

    public Animator thisCatAnimator;
	protected NavMeshAgent thisNavMeshAgent;
	protected CatStates thisCatState;
    float catRunSpeed = .02f;
    public float triggerFollowDistance = 15f;
    float catLookAngle = 20;
    float waypointToggleDistance = 3f;
	//cat runs back to this point after it attacks player
	Vector3 runawayTarget;

	public GameObject deadCat;

	float overlapTimeTilDeath;
	public bool isKillable;
	bool isOverlappingPlayer;
	protected bool canCauseStress=true;

    public GameObject[] waypoints;
    int waypointIndex;
    // Use this for initialization

	[SerializeField]
    bool switchToFollowing, switchToIdle, switchToWaypoints, switchToRunaway, switchToAttackPlayer;
	protected void Start () {

		thisNavMeshAgent = GetComponent<NavMeshAgent> ();

		if (switchToFollowing) {
			SwitchToState (CatStates.Following);
			switchToFollowing = false;
		}
		else if (switchToAttackPlayer) {
			SwitchToState(CatStates.AttackPlayer);
			switchToAttackPlayer = false;
		}
		else if (switchToIdle) {
			thisCatState = (CatStates.Idle);
			switchToIdle = false;
		}
		else {
			SwitchToState (CatStates.Waypoints);
		}
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
				switchToIdle = true;
			}
			if (switchToIdle) {
				SwitchToState (CatStates.Idle);
				switchToIdle = false;
			}
			else if (switchToRunaway) {
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
				if (CheckIsPlayerLookingAtCat ()) {
					switchToFollowing = false;
					SwitchToState (CatStates.Idle);
				} else {
					switchToFollowing = false;
					SwitchToState (CatStates.Following);
				}
			}
				
			break;
		case CatStates.AttackPlayer:
			FollowPlayer ();
			break;
        }

	}
		


	public virtual void Interact () {
		if (Vector3.Distance (transform.position, TestPlayerController.s_instance.transform.position) < 1.3f) {
			
			TestPlayerController.s_instance.Stomp ();
			StartCoroutine ("StompCat");
			TestPlayerController.s_instance.PlayBark ();

		}
	}

	IEnumerator StompCat() {
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.catkill2);
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.catkill4);

		yield return new WaitForSeconds (.2f);
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.catkill1);
		SoundtrackManager.s_instance.PlayAudioSource (SoundtrackManager.s_instance.catkill3);

		Instantiate (deadCat, transform.position, transform.rotation);
		DestroyCat ();
	}

    void FollowPlayer()
    {
		//transform.LookAt(TestPlayerController.s_instance.transform.position);
		thisNavMeshAgent.SetDestination (TestPlayerController.s_instance.transform.position);

        
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
		if (Vector3.Distance(transform.position, runawayTarget) < .1f) {
			switchToFollowing = true;
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

    float GetAngleBetweenPlayerForwardAndCat()
	{
		Vector3 playerForward = TestPlayerController.s_instance.transform.forward;
		Vector3 towardCat = -TestPlayerController.s_instance.transform.position + transform.position;
        return Vector3.Angle(playerForward, towardCat);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && canCauseStress)
        {
			//if (thisCatState != CatStates.Waypoints&&thisCatState != CatStates.Talking) {
			if (thisCatState == CatStates.AttackPlayer && !GameManager.s_instance.saveWomen) { //only used for clifford super HACK	
				TestPlayerController.s_instance.SetPlayerModeCutscene ();
				GetComponent<CinemachineHardCut> ().HardCut ();
				GameObject.Find ("HELLCAM").GetComponent<Cinemachine.CinemachineVirtualCamera> ().enabled = true;
				GameObject.Find ("HELLCAM").GetComponent<AudioSource> ().enabled = true;
				GameObject.Find ("HELLCAM").GetComponent<AudioListener> ().enabled = true;


				GameObject.Find ("NaughtyP_Bettter").GetComponent<Animator>().enabled = (true);
				GameObject.Find ("NaughtyP_Bettter").GetComponent<TimedEventTrigger>().enabled = (true);
				TestPlayerController.s_instance.gameObject.SetActive (false);
				OverlayManager.s_instance.blackout.SetActive (true);
				gameObject.SetActive (false);
			}
			else {
				isOverlappingPlayer = true;
				TestPlayerController.s_instance.GetComponent<HealthHandler> ().TakeStress (5);
				switchToRunaway = true;
			//}
			}
        }
    }

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			isOverlappingPlayer = false;
		}
	}


	public void SwitchToState(CatStates switchToThisState) {
		switch (switchToThisState) {
		case CatStates.Waypoints:
			thisCatState = CatStates.Waypoints;
			thisCatAnimator.SetTrigger("run");
			thisNavMeshAgent.isStopped = false;
			GotoNextWaypoint ();
			break;

		case CatStates.Idle:
			thisCatState = CatStates.Idle;
			thisCatAnimator.ResetTrigger ("run");
			thisCatAnimator.SetTrigger ("idle");
			thisNavMeshAgent.isStopped = true;
			break;
		case CatStates.Following:
			thisCatState = CatStates.Following;
			thisCatAnimator.SetTrigger ("run");
			thisNavMeshAgent.isStopped = false;
			runawayTarget = transform.position;
			break;
		case CatStates.Runaway:
			thisCatState = CatStates.Runaway;
			break;
		case CatStates.AttackPlayer:
			thisCatState = CatStates.AttackPlayer;
			thisCatAnimator.SetTrigger ("run");
			break;
		}
	}


    protected void DestroyCat()
    {
		TestPlayerController.s_instance.GetComponent<HealthHandler> ().TakeStress (1);
        Destroy(gameObject);
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
}
