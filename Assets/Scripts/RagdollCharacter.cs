using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollCharacter : MonoBehaviour {

	Collider[] ragdollColliders;

	void Start()
	{
		GetRigidbodies();
	}

	public void GetRigidbodies() {
		ragdollColliders = GetComponentsInChildren<Collider>();
		foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
		{
			rb.isKinematic = true;
			rb.useGravity = false;
			rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		}
//		foreach (Collider col in GetComponentsInChildren<Collider>())
//		{
//			col.enabled = false;
//		}
	}

	/// <summary>
	/// Activate rigidbody at point with force.
	/// </summary>
	/// <param name="force">Force.</param>
	/// <param name="point">Point.</param>
	public void Activate(Vector3 point, Vector3 force) {
		GetRigidbodies();

		GetComponent<Animator>().enabled = false;

		Collider nearest = GetNearestColliderOnRagdoll(point);
		Rigidbody nearestRB = nearest.GetComponent<Rigidbody>();
		if (nearestRB != null)
		{
			Activate(nearestRB, point, force);
		}
	}

	/// <summary>
	/// Activate rigidbody with specified hitRB, at point with force.
	/// </summary>
	/// <param name="hitRB">Hit R.</param>
	/// <param name="point">Point.</param>
	/// <param name="force">Force.</param>
	public void Activate(Rigidbody hitRB, Vector3 point, Vector3 force) {
		Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rb in rigidbodies)
		{
			rb.isKinematic = false;
			rb.useGravity = true;
			if (rb.GetComponent<CharacterJoint>())
			{
				rb.GetComponent<CharacterJoint>().enableProjection = true;
			}
		}

		foreach (Collider col in GetComponentsInChildren<Collider>())
		{
			col.enabled = true;
		}
		if (GetComponent<CharacterController>() != null)
			GetComponent<CharacterController>().enabled = false;
		if (force != Vector3.zero)
			StartCoroutine(ForceOverTime(hitRB, force, point));
	}

	public void AddForce(Vector3 point, Vector3 force) {
		Collider nearest = GetNearestColliderOnRagdoll(point);
		Rigidbody nearestRB = nearest.GetComponent<Rigidbody>();
		if (nearestRB != null)
		{
			Activate(nearestRB, point, force);
		}
	}

	IEnumerator ForceOverTime(Rigidbody rb, Vector3 force, Vector3 point) {
		float exponentialFalloff = 0.5f;
		for (float e = 1f; e > 0.01f; e *= exponentialFalloff)
		{
			//            foreach(Rigidbody rb in rigidbodies) {
			rb.AddForceAtPosition(force * e, point);
			//            }
			yield return null;
		}
	}

	Collider GetNearestColliderOnRagdoll(Vector3 point) {
		if (ragdollColliders.Length == 0)
		{
			Debug.Log("Ragdoll has no child colliders");
			return null;
		}
		//Get nearest collider to point
		Collider nearestCol = ragdollColliders[0];
		for (int i = 1; i < ragdollColliders.Length; i++)
		{
			float minDist = Vector3.Distance(point, nearestCol.transform.position);
			float newDist = Vector3.Distance(point, ragdollColliders[i].transform.position);
			if (newDist < minDist)
			{
				nearestCol = ragdollColliders[i];
			}
		}
		return nearestCol;
	}
}
