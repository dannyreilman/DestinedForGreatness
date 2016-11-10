using UnityEngine;
using System.Collections;

public abstract class Movement : MonoBehaviour {
    public bool inverted = false;
    public Vector2 goalLoc, currentLoc;
    public Transform movingObjectTransform;
    public Animator mainAnimator;
    public int state = 1;
    public const int STATIONARY = 0;
    public const int MOVING = 1;
    public const int FOLLOWING = 2;
    public bool cinematic;

    public Vector2 spacingOffset;
    public Transform followTransform = null;

    void Awake()
    {
        currentLoc = new Vector2(0, 0);
    }

	public void CallableUpdate (Vector2 xy)
    {
        currentLoc = xy;
        switch (state)
        {
        case STATIONARY:
                if(!cinematic)
                    mainAnimator.SetFloat("AttackSpeed", 1);
                else
                    mainAnimator.SetFloat("AttackSpeed", 0);
                break;
        case FOLLOWING:
                if (!(followTransform == null || followTransform.Equals(null)))
                {
                    if (!cinematic)
                        mainAnimator.SetFloat("AttackSpeed", 1);
                    else
                        mainAnimator.SetFloat("AttackSpeed", 0);
                    Vector2 sum = AddPlayerOffset(followTransform.position);
                    goalLoc = sum;
                    Vector2 totalDistance = goalLoc - currentLoc;
                    if(totalDistance.magnitude > spacingOffset.magnitude && !cinematic)
                    {
                        Move();
                    }
                }
                else
                {
                    state = STATIONARY;
                }
            break;
        case MOVING:
                mainAnimator.SetFloat("AttackSpeed", 0);
                if(!cinematic)
                    Move();
            break;
        }
        movingObjectTransform.position = new Vector3(currentLoc.x, currentLoc.y, movingObjectTransform.position.z);
    }

    public void SetPosition(Vector2 pos)
    {
        state = STATIONARY;
        transform.position = pos;
        goalLoc = pos;
        currentLoc = pos;
        movingObjectTransform.position = new Vector3(currentLoc.x, currentLoc.y, movingObjectTransform.position.z);
    }

    public void Reset()
    {
        goalLoc = transform.position;
        state = MOVING;
        followTransform = null;
    }
    
    public void InstantReset()
    {
        SetPosition(transform.position);
    }

    public abstract void Move();

    public void MoveToLoc(Vector2 pos)
    {
        goalLoc = pos;
        state = MOVING;
    }

    public void MoveToPlayer(Transform player)
    {
        MoveToLoc(AddPlayerOffset(player.position));
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
