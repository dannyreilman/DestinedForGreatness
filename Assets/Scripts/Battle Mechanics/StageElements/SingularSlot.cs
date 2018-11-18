using System.Collections.Generic;
using UnityEngine;

public class SingularSlot : Slot{

	public SingularEntity entity = null;
	
	//Minor entities are any non-singular entities that are also added to this slot
	//Minor entities may include shields and other uncontrollable elements
	public List<SlotEntity> minorEntities;
	public float width;
	//Move a new entity into this slot


	public override void moveEntity(SlotEntity entity_in)
	{
		if(entity_in is SingularEntity && entity_in != entity)
		{
			//Bump off last entity
			if(entity != null)
				entity.Bump(entity_in.container);
			
			//Set new entity
			entity_in.container = this;
			entity = (SingularEntity)entity_in;
		}
		else
		{
			minorEntities.Add(entity_in);
			entity_in.container = this;
		}

		entity_in.side = friendly;
	}

	public override SlotEntity GetTarget()
	{
		if(entity != null)
		{
			return entity;
		}
		else if(minorEntities.Count > 0)
		{
			return minorEntities[Random.Range(0, minorEntities.Count )];	
		}
		else
		{
			return null;
		}
	}

	public override Vector2 GetLocation(SlotEntity s)
	{
		if(s == entity)
		{
			return transform.position;
		}

		return new Vector2(Random.Range(-1,1) * width +  transform.position.x, Random.Range(-1,1) * Slot.WIGGLE_ROOM + transform.position.y);
	}


	public override bool PoliteMoveable()
	{
		return (entity == null || entity.Equals(null));
	}


	public override void AoeAttack(Attack a)
	{
		entity.TakeAttack(a);
		foreach(SlotEntity s in minorEntities)
		{
			s.TakeAttack(a);
		}
	}


	public override bool Targetable()
	{
		return entity != null || minorEntities.Count > 0;
	}

}