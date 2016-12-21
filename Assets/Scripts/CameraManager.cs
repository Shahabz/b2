using UnityEngine;
using System.Collections;

/// <summary>
/// The camera manager is used to handle and store references to different camera positions and states
/// through out the game, for instance, dolly camera, rising camera, or last camera position before
/// entering a certain state
/// </summary>

public class CameraManager : MonoBehaviour {

    [SerializeField]
    Camera anxietyCam, jobCam;

    Vector3 anxietyCamStartPosition;

    Vector3 lastInLevelCamPosition;
    Quaternion lastInLevelCamRotation;

    Vector3 preTransitionCamPosition;
    Quaternion preTransitionCamRotation;

    Vector3 mainSceneViewPosition;
    Quaternion mainSceneViewRotation;
    
    public static CameraManager s_instance;

    void Awake ()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start () {
        anxietyCamStartPosition = anxietyCam.transform.localPosition;

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void SetMainViewOnScene(Camera thisCamera)
    {
        mainSceneViewPosition = thisCamera.transform.position;
        mainSceneViewRotation = thisCamera.transform.rotation;
    }

    public void SwitchToAnxietyCam()
    {
        Camera.main.transform.rotation = anxietyCam.transform.rotation;
        Camera.main.transform.position = anxietyCam.transform.position;
    }

    public void SwitchToJobCam() {
        Camera.main.transform.rotation = jobCam.transform.rotation;
        Camera.main.transform.position = jobCam.transform.position;
    }

    public void ResetAnxietyCam ()
    {
        anxietyCam.transform.localPosition = anxietyCamStartPosition;
    }

    public void RaiseCamera(float speed)
    {
        Camera.main.transform.Translate(Vector3.up * Time.deltaTime * speed);

    }

    public void MultipleChoiceCameraOn()
    {
        Camera.main.transform.rotation = mainSceneViewRotation;
        Camera.main.transform.position = mainSceneViewPosition;
    }

    public void SetLastInLevelCamTransform()
    {
        lastInLevelCamPosition = Camera.main.transform.position;
        lastInLevelCamRotation = Camera.main.transform.rotation;
    }

    // Moves camera to the camera position of where you were when you entered the multiple choice mode
    public void MultipleChoiceCameraOff()
    {
        Camera.main.transform.rotation = lastInLevelCamRotation;
        Camera.main.transform.position = lastInLevelCamPosition;
    }

    public void SetCamera(Camera thisCamera)
    {
        Camera.main.transform.rotation = thisCamera.transform.rotation;
        Camera.main.transform.position = thisCamera.transform.position;
    }
    public void SetPreTransitionCamera (Camera thisCamera)
    {
        preTransitionCamPosition = thisCamera.transform.position;
        preTransitionCamRotation = thisCamera.transform.rotation;
    }
}
