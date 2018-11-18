using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMovement : MonoBehaviour 
{
	public float width;
	ChangeLocationScript toFollow;

	void Awake()
	{
		toFollow = transform.parent.GetChild(0).GetComponent<ChangeLocationScript>();
	}

	// Update is called once per frame
	void Update () 
	{
		transform.position = toFollow.GetMovementLocation();
		transform.rotation = Camera.main.transform.rotation;
	}
}
