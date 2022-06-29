using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadingAngleTracker : MonoBehaviour {

    public GameObject colliderSphere;
	public GameObject homePost;
    public Vector3 playerHeading;
	public Vector3 homePostLoc;
	public float heading;
	public float headingAngleError;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ColliderSp"))
        {
			headingAngleError = heading;
        }
    }

	void Update()
	{
		playerHeading = colliderSphere.transform.position;
		playerHeading.y = 0.0f;
		homePostLoc = homePost.transform.position;
		heading = Vector3.SignedAngle (homePostLoc, playerHeading, Vector3.up);
	}
}
