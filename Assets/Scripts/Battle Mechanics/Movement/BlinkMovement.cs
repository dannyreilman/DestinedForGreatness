using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkMovement : MovementType
{
	public override bool Move(ref Vector2 xy, Vector2 target)
	{
		xy = target;
		return true;
	}

	public override MovementType Copy()
	{
		return new BlinkMovement();
	}
}
