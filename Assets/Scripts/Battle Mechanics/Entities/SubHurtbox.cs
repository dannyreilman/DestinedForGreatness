using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubHurtbox : MonoBehaviour 
{
	public Attack attack;
	public bool active = true;
	public bool friendly;

	void OnCollisionEnter2D(Collision2D coll) 
	{
		if(active)
		{
			SlotEntity target = coll.gameObject.GetComponent<SlotEntity>();
			if(target && target.side != friendly)
				target.TakeAttack(attack);
		}
	}

}
