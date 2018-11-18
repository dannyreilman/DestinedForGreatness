using UnityEngine;
using System.Collections.Generic;

/**
 * A slot. Slots contain slot entities, and determine where slot entites
 * are placed. This class is meant to be implemented
 */
public abstract class Slot : MonoBehaviour {
	public static Dictionary<string, Slot> ALLY_SLOTS = null;
	public static Dictionary<string, Slot> ENEMY_SLOTS = null;

	public int slotLayer;

	public static float WIGGLE_ROOM = 0.1f;
	public enum Type
	{
		Grounded,
		Air,
		Underground,
		Underwater
	}

	public bool friendly;
	
	public Type type;

	public string slotName;

	void Awake()
	{
		if(ALLY_SLOTS == null)
		{
			ALLY_SLOTS = new Dictionary<string, Slot>();
		}

		if(ENEMY_SLOTS == null)
		{
			ENEMY_SLOTS = new Dictionary<string, Slot>();
		}

		if(friendly)
		{
			ALLY_SLOTS.Add(slotName, this);
		}
		else
		{
			ENEMY_SLOTS.Add(slotName, this);
		}
	}


	//Move an entity to this slot, swapping if necessary
	public abstract void moveEntity(SlotEntity entity_in);

	//Get a target from the slot
	public abstract bool Targetable();

	public abstract SlotEntity GetTarget();

	//Return a location suitable for the given SlotEntity
	public abstract Vector2 GetLocation(SlotEntity s);

	public int GetLayer()
	{
		return slotLayer;
	}

	//Hit all entities
	public abstract void AoeAttack(Attack a);

	//Ask politely if this slot can be moved to (as in without bumping)
	public virtual bool PoliteMoveable()
	{
		//By default all slots are move-toable
		return true;
	}
}
