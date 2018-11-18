using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MovementType 
{
	float timePassed = 0;
	float delayTime = 0;

	public Pause(float delay)
	{
		delayTime = delay;
	}

	public override bool Move(ref Vector2 xy, Vector2 target)
	{
		timePassed += Time.deltaTime;
		return timePassed > delayTime;
	}


	public override MovementType Copy()
    {
        return new Pause(delayTime);
    }

}
