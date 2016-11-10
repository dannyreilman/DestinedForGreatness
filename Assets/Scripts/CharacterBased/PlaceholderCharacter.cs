using UnityEngine;
using System.Collections;
using System;

public class PlaceholderCharacter : Character
{

    protected override void SetBaseStats()
    {
        baseStats.maxHp = 1;
        baseStats.attack = 1;
        baseStats.maxArmor = 1;
        baseStats.speed = 10;
    }

    protected override void DoAttack1()
    {
        Debug.Log("Attack 2");
    }

    protected override void DoAttack2()
    {
        Debug.Log("Attack 2");
    }

    protected override void DoAttack3()
    {
        Debug.Log("Attack 3");
    }

    protected override void DoAttack4()
    {
        Debug.Log("Attack 4");
    }

    protected override void Death()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        base.Death();
    }
}
