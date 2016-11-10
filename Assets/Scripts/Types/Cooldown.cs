using UnityEngine;

public class Cooldown
{
    public float duration;
    public float timeSoFar;
    public int attack;

    public bool canAttack
    {
        get
        {
            return timeSoFar > duration;
        }
    }

    public float percent
    {
        get
        {
            return Mathf.Min(timeSoFar / duration,1);
        }
    }

    public Cooldown(int attack, float duration)
    {
        this.attack = attack;
        if (duration != 0)
        {
            this.duration = duration;
        }
        else
        {
            this.duration = 0.001f;
        }
        timeSoFar = 0;
    }

    public void StartCooldown(float duration)
    {
        this.duration = duration;
        timeSoFar = 0;
    }

    public void Update(float time)
    {
        timeSoFar += time;
    }
}
