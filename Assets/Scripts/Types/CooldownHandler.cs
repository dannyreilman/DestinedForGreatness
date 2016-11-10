using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CooldownHandler
{
    public Cooldown[] cooldowns;

    public CooldownHandler()
    {
        cooldowns = new [] { new Cooldown(1, 0),
                            new Cooldown(2, 0),
                            new Cooldown(3, 0),
                            new Cooldown(4, 0)};
    }

    public void StartCooldown(int attack, float duration)
    {
        cooldowns[attack - 1].StartCooldown(duration);
    }

    public void Update()
    {
        for(int i = cooldowns.GetLength(0); i >=0 ; i--)
        {
            cooldowns[i].Update(Time.deltaTime);
        }
    }

    public float TimeLeft(int attack)
    {
        return cooldowns[attack - 1].duration;
    }

    public float PercentLeft(int attack)
    {
        return cooldowns[attack - 1].percent;
    }
}
