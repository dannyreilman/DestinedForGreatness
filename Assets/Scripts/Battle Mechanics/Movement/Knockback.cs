using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MovementType {
	
	const float GET_UP_DIST = 10f;
	const float GET_UP_PERIOD = 0.25f;

	Vector2 velocity;
	int layer;

	int state;
	const int FLYING = 0;
	const int LANDED = 1;


	public Knockback(Vector2 amount, int layer_in)
	{
		velocity = amount;
		layer = layer_in;
		state = 0;
	}

	public override MovementType Copy()
	{
		return new Knockback(velocity, layer);
	}

	float landedTimePassed = 0;
	public override bool Move(ref Vector2 xy, Vector2 target)
	{
		switch(state)
		{
			case FLYING:
				xy += velocity * Time.deltaTime;
				velocity += MovementType.GRAVITY * Time.deltaTime;

				if(xy.y <= ChangeLocationScript.layerVerticalOffsets[layer] - GET_UP_DIST)
				{
					state = LANDED;
					velocity.x = 0;
				}
				return false;

			case LANDED:
				landedTimePassed += Time.deltaTime;

				xy.y = ChangeLocationScript.layerVerticalOffsets[layer] - GET_UP_DIST * (1 - (landedTimePassed / GET_UP_PERIOD));

				if(landedTimePassed >= GET_UP_PERIOD)
				{
					xy.y = ChangeLocationScript.layerVerticalOffsets[layer];
					return true;
				}
				else
				{
					return false;
				}
		}

		return false;
	}

}
