using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideSlot : Slot
{
	public float width;
	public float height;

	private List<SlotEntity> entities;

	public override void moveEntity(SlotEntity entity_in)
	{
		entities.Add(entity_in);
		entity_in.side = friendly;
	}

	public override SlotEntity GetTarget()
	{
		return entities[Random.Range( 0, entities.Count )];	
	}

	public override Vector2 GetLocation(SlotEntity s)
	{
		return new Vector2(Random.Range(-1,1) * width +  transform.position.x, Random.Range(-1,1) * Slot.WIGGLE_ROOM + transform.position.y);
	}


	public override void AoeAttack(Attack a)
	{
		foreach(SlotEntity s in entities)
		{
			s.TakeAttack(a);
		}
	}

	public override bool Targetable()
	{
		return entities.Count > 0;
	}
}