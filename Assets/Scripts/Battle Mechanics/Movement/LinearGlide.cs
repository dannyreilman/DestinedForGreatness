using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

//Moves directly to the target
public class LinearGlide : MovementType
{

    public LinearGlide(float speed)
    {
        this.speed = speed;
    }

    private float speed;

    public override bool Move(ref Vector2 currentLoc, Vector2 target)
    {
        //If not at goal location, move
        if (!Mathf.Approximately(target.x, currentLoc.x) || !Mathf.Approximately(target.y, currentLoc.y))
        {
            Vector2 fullDirection = target - currentLoc;
            Vector2 direction = fullDirection / fullDirection.magnitude;
            Vector2 movingVector = direction * speed * Time.deltaTime;
            if ((movingVector).magnitude >= fullDirection.magnitude)
            {
                currentLoc = target;
            }
            else
            {
                currentLoc += movingVector;
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    public override MovementType Copy()
    {
        return new LinearGlide(speed);
    }
}
