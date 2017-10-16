using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;

	float distance = 5.0f;
	[SerializeField]
	float xSpeed = 120.0f;
	[SerializeField]
	float ySpeed = 120.0f;

	[SerializeField]
	float yMinLimit = -20f;
	[SerializeField]
	float yMaxLimit = 80f;

	[SerializeField]
	float distanceMin = .5f;
	[SerializeField]
	float idleDistanceMax = 1f;
	[SerializeField]
	float zoomedDistanceMax = 2f;

	float currentMaxDistance = 0f;

	[HideInInspector]
	public bool zoomedIn = false;
	
	float x = 0.0f;
	float y = 0.0f;

	public LayerMask collisionMask;

	public float clipBuffer = 0.1f;

	PlayerInput input;

	void Start () {
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;

		input = target.GetComponentInParent<PlayerInput>(); //Cuz we're looking at a child object ;)
		if(input == null) {
			Debug.LogError("PlayerInput is null", this);
			this.enabled = false;
			return;
		}
	}

	Vector3 HandleCollisionZoom() {

		Vector3 camOut = Vector3.Normalize(target.position - transform.position);

		currentMaxDistance = Mathf.Lerp(currentMaxDistance, (zoomedIn ? idleDistanceMax : zoomedDistanceMax), Time.deltaTime * 5f);
		Vector3 maxCamPos = target.position - (camOut * currentMaxDistance);

		float minHitDistance = 9999f; 

		Vector3[] corners = GetNearPlaneCorners();
		for (int i = 0; i < 4; i++)
		{
			Vector3 offsetToCorner = corners[i] - transform.position;
			RaycastHit hit;
			Vector3 rayEnd = maxCamPos + offsetToCorner;
			Debug.DrawLine(target.position + offsetToCorner, rayEnd, Color.red);
			if(Physics.Linecast(target.position + offsetToCorner, rayEnd, out hit, collisionMask)) {
				if(Vector3.Distance(hit.point, target.position) > 0.5f)
					minHitDistance = hit.distance;
			}
		}
		if(minHitDistance < 9999f)
			return target.position - (camOut * (1 - clipBuffer) * minHitDistance);
		else
			return maxCamPos;
	}

	Vector3[] GetNearPlaneCorners() {
		Vector3[] nearClipCorners = new Vector3[4];
		 
		float nearH = 2 * Mathf.Tan (GetComponent<Camera>().fieldOfView * Mathf.Deg2Rad / 2.0f) * GetComponent<Camera>().nearClipPlane;
		float nearW = nearH * GetComponent<Camera>().aspect;

		Vector3 nearC = transform.position + transform.forward * GetComponent<Camera>().nearClipPlane;

		nearClipCorners [0] = nearC + (transform.up * (nearH / 2.0f)) - (transform.right * (nearW / 2.0f));
		nearClipCorners [1] = nearC + (transform.up * (nearH / 2.0f)) + (transform.right * (nearW / 2.0f));
		nearClipCorners [2] = nearC - (transform.up * (nearH / 2.0f)) - (transform.right * (nearW / 2.0f));
		nearClipCorners [3] = nearC - (transform.up * (nearH / 2.0f)) + (transform.right * (nearW / 2.0f));

		return nearClipCorners;
	}

	void LateUpdate () {
		if(TestPlayerController.s_instance.lockInput == TestPlayerController.InputLock.Locked)
			return;
		if (target) {
			Vector3 lookPos = target.position;

			x += input.lookDir.x * xSpeed * distance * 0.02f;
			y -= input.lookDir.y * ySpeed * 0.02f;

			y = ClampAngle(y, yMinLimit, yMaxLimit);

			//Quaternion rotation = Quaternion.Euler(y, x, 0);
			Quaternion rotation = Quaternion.Euler(y, x, 0);

			Debug.DrawRay(HandleCollisionZoom(), new Vector3(0f, 0f, 0f), Color.blue);
			distance = Vector3.Distance(HandleCollisionZoom(), target.position);

			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + lookPos;

			transform.rotation = rotation;
			transform.position = position;	

//			Vector3 endPos = transform.Find("LaserEnd").localPosition;
//			transform.Find("LaserEnd").localPosition = Vector3.Lerp(endPos, new Vector3(0f, 0f, endPos.z), 20f*Time.deltaTime);
		}
	}
	
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}

	public void Recoil(float recoil) {
		transform.Find("LaserEnd").localPosition += new Vector3(0f, recoil, 0f);
	}
}
