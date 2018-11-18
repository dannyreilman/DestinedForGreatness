using UnityEngine;
using System.Collections;

public abstract class MovementType {
	public static float HIT_THRESH = 0.05f;
	public static Vector2 GRAVITY = new Vector2(0, -400);

	public static bool DistanceStopping(Vector2 xy, Vector2 target, float dist)
	{
		return (xy - target).magnitude <= dist;
	}

	public static bool DistanceStopping(Vector2 xy, Vector2 target)
	{
		return DistanceStopping(xy,target, HIT_THRESH);
	}

    //Returns whether the movement is "finished"
	public abstract bool Move (ref Vector2 xy, Vector2 target);

	public abstract MovementType Copy();

}
