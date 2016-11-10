using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class ConstantMovement : Movement
{
    public float speed;
    public override void Move()
    {
        if (!Mathf.Approximately(goalLoc.x, currentLoc.x) || !Mathf.Approximately(goalLoc.y, currentLoc.y))
        {
            Vector2 fullDirection = goalLoc - currentLoc;
            Vector2 direction = fullDirection/fullDirection.magnitude;
            Vector2 movingVector = direction * speed * Time.deltaTime;
            if ((movingVector).magnitude >= fullDirection.magnitude)
            {
                currentLoc = goalLoc;
            }
            else
            {
                currentLoc += movingVector;
            }
        }
        else
        {
            if(followTransform == null || followTransform.Equals(null))
            {
                state = STATIONARY;
            }
            else
            {
                state = FOLLOWING;
            }
        }
    }

    public void MoveToAndFollowPlayer(Transform t)
    {
        followTransform = t.GetComponent<Character>().mover.movingObjectTransform;
        state = FOLLOWING;
    }
}
