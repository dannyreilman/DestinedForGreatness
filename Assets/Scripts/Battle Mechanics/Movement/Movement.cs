using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public static float OUT_OF_SIGHT = -30;
	
	private class Bundle
	{
		public MovementType style;
		public Transform targetTransform;
		public Vector2 targetPoint;

		public Bundle Copy()
		{
			Bundle toReturn = new Bundle();
			toReturn.style = style.Copy();
			toReturn.targetTransform = targetTransform;
			toReturn.targetPoint = targetPoint;

			return toReturn;
		}
	}

    public bool inverted = false;
    private Vector2 target;
    private Animator mainAnimator;
    private Animator sideAnimator;

	private LinkedList<Bundle> states = new LinkedList<Bundle>();

	private Bundle defaultState;
	private Bundle defaultCopy;
    public bool cinematic;

    public Vector2 spacingOffset;

	private int targetLayer;

	private ChangeLocationScript changeLoc;

	private Character c;


	void Start()
	{
		changeLoc = GetComponent<ChangeLocationScript>();
		mainAnimator = GetComponent<Animator>();
		sideAnimator = transform.parent.GetComponent<Animator>();
	}

	public void SetDefault(MovementType style, Character c)
	{
		defaultState = new Bundle();
		defaultState.style = style;
		defaultState.targetTransform = null;
		this.c = c;
		defaultState.targetPoint = c.container.GetLocation(c);
		defaultCopy = defaultState.Copy();
	}

	void Update ()
    {
		mainAnimator.SetFloat("AttackSpeed", 1);
		if(states.Count > 0)
		{	
			if(c != null)
				c.moving = true;

			if(defaultCopy == null && defaultState != null)
			{
				defaultCopy = defaultState.Copy();	
			}

			if(!(states.First.Value.targetTransform == null || states.First.Value.targetTransform.Equals(null)))
			{
				target = AddPlayerOffset(states.First.Value.targetTransform.position);
			}
			else
			{
				target = states.First.Value.targetPoint;
			}

			bool finished = states.First.Value.style.Move(ref changeLoc.movementLocation, target);
		
			if(finished)
			{
				Debug.Log("Finished");

				if(sideAnimator.enabled)
					sideAnimator.SetTrigger("MovementFinished");
				states.RemoveFirst();
			}
		}
		else
		{
			if(defaultCopy != null)
			{
				if(c != null)
					c.moving = true;
				ChangeLayer(c.container.slotLayer);
				if(defaultCopy.style.Move(ref changeLoc.movementLocation, defaultCopy.targetPoint))
				{
					defaultState.targetPoint = c.container.GetLocation(c);
					defaultCopy = null;
					StartCoroutine(CopyAfterDelay());
				}
			}
			else
			{
				if(c != null)
					c.moving = false;
			}
		}

		if(changeLoc.GetActualPosition().y < OUT_OF_SIGHT || 
			Mathf.Abs(changeLoc.GetActualPosition().x) < ChangeLocationScript.outOfSightPos[changeLoc.layer])
		{
			changeLoc.layer = targetLayer;
		}

    }

	private IEnumerator CopyAfterDelay()
	{
		yield return new WaitForSeconds(5.0f);
		if(defaultCopy == null && defaultState != null)
		{
			defaultCopy = defaultState.Copy();
		}
	}

	public void ChangeLayer(int layer)
	{
		targetLayer = layer;
	}

    public void MoveToTransform(Transform player, MovementType style, int priority = 0)
    {
		Bundle toPush = new Bundle();
		toPush.style = style;
		toPush.targetTransform = player;
		toPush.targetPoint = AddPlayerOffset(player.position);
		switch(priority)
		{
			case 0:
				states.AddLast(toPush);
				break;
			case 1:
				states.AddFirst(toPush);
				break;
			case 2:
				states.Clear();
				states.AddFirst(toPush);
				break;
		}
    }

	public void MoveToLocation(Vector2 location, MovementType style, int priority = 0)
	{
		Bundle toPush = new Bundle();
		toPush.style = style;
		toPush.targetTransform = null;
		toPush.targetPoint = AddPlayerOffset(location);
		switch(priority)
		{
			case 0:
				states.AddLast(toPush);
				break;
			case 1:
				states.AddFirst(toPush);
				break;
			case 2:
				states.Clear();
				states.AddFirst(toPush);
				break;
		}
	}
    private Vector2 AddPlayerOffset(Vector2 pos)
    {
        if(inverted)
        {
            return pos + new Vector2(-spacingOffset.x, spacingOffset.y);
        }
        return pos + spacingOffset;
	}

}
