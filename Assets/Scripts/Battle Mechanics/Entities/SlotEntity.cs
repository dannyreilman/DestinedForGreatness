using UnityEngine;

//Anything that could be put in a slot
public abstract class SlotEntity : MonoBehaviour
{
	public Slot container;
	public Slot defaultContainer;

	//Side = true means friendly, false means enemy
	//friendly = left, enemy = right
	private bool side_internal;
	public virtual bool side
	{
		get
		{
			return side_internal;
		}
		set
		{
			if(side_internal != value)
			{
				side_internal = !side_internal;
				UpdateFlip();
			}
		}
	}

	
	//Changes side
	public abstract void UpdateFlip();
	public abstract void TakeAttack(Attack a);

	public abstract bool CanBeTargeted();

	public virtual Transform GetTransform()
	{
		return transform;
	}
}
