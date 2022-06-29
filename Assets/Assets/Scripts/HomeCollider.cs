using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCollider : MonoBehaviour {

	public GameObject homePost;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("ColliderSp"))
		{
			homePost.SetActive(false);
		}
	}
}
