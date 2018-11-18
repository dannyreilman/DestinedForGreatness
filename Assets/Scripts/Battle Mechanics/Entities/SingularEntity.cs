using UnityEngine;

//A singularEntity is a slotEntity that can sit in a singularSlot
//This entity must be able to take attacks and give information on whether it can be targeted
//may sometimes do nothing when DoAttack is called
public abstract class SingularEntity : SlotEntity
{
    SpriteRenderer sprite;

    public Vector2 target
    {
        get 
        {
            return transform.position;
        }
    }

    //Defines how this entity will react to being forced off their 
    //Slot. Default is swap with the Bump-er
    public virtual void Bump(Slot newContainer)
    {
        if((newContainer is SingularSlot) 
            &&((SingularSlot)newContainer).entity != null)
        {
            //Bump to default container 
            container = defaultContainer;
        }
        else
        {
            //Swap with the Bump-er if you can
            container = newContainer;
        }
    }

    public override void UpdateFlip()
    {
        sprite.flipX = side;
    }

    public abstract void DoAttack(int attack);
    public abstract bool CanAttack();

	public abstract void Hold(bool holding);

    public abstract bool IsAlive();
}