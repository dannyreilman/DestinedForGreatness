using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstHitbox : MonoBehaviour {

	private int framesLeft = 0;
	private SubHurtbox[] boxes; 

	public Attack a;

	// Use this for initialization
	void Awake () {
		boxes = GetComponentsInChildren<SubHurtbox>();
		foreach(SubHurtbox sub in boxes)
		{
			sub.active = false;
		}
	}

	void Update()
	{
		if(framesLeft > 0)
		{
			--framesLeft;
			if(framesLeft == 0)
			{
				foreach(SubHurtbox sub in boxes)
				{
					sub.active = false;
				}
			}
		}
	}

	public void Trigger(int frames = 2)
	{
		framesLeft = frames;

		foreach(SubHurtbox sub in boxes)
		{
			sub.active = true;
		}
	}

	
	
}
