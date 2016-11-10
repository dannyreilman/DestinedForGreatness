using UnityEngine;
using System.Collections;
using System;

public class EmptyCharacter : Character {

    override public bool IsAlive()
    {
        return false;
    }

    override protected void DoAttack1() { }
    override protected void DoAttack2() { }
    override protected void DoAttack3() { }
    override protected void DoAttack4() { }

    protected override void SetBaseStats()
    {
        baseStats.maxHp = 0;
        baseStats.maxArmor = 0;
        baseStats.attack = 0;
        baseStats.speed = 1;
    }
}
