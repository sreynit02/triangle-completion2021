using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutCollider : MonoBehaviour {

	public GameObject outBoundPost;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("ColliderSp"))
		{
			outBoundPost.SetActive(false);
		}
	}
}
