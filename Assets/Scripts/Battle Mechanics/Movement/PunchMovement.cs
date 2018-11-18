using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchMovement : MovementType
{
	public static float YVEL = 5f;

	public PunchMovement(float acceleration)
	{
		accel = acceleration;
	}

	private float accel;

	private float ramp = 0;
	private float velocity = 0.0f;

	public override bool Move(ref Vector2 xy, Vector2 target)
	{
		ramp += 25f * Time.deltaTime;

		if((xy.x >= target.x) ^ (velocity < 0))
		{
			velocity = 0;
			ramp = 0f;
			xy = target;
			return true;
		}
		

		if(Mathf.Abs(xy.y - target.y) < YVEL * Time.deltaTime * ramp)
		{
			xy.y = target.y;
		}
		else
		{
			if(xy.y < target.y)
			{
				xy.y += YVEL * Time.deltaTime * ramp;
			}
			else
			{
				xy.y -= YVEL * Time.deltaTime * ramp;
			}
			
		}
		
		if(xy.x < target.x)
		{
			velocity += accel * ramp * Time.deltaTime;
		}
		else
		{
			velocity -= accel * ramp * Time.deltaTime;
		}

		xy.x += velocity * Time.deltaTime;

		return MovementType.DistanceStopping(xy,target);
	}

	public override MovementType Copy()
    {
        return new PunchMovement(accel);
    }

}
