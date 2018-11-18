using UnityEngine;
using System.Collections;

public class UntargetableTillAttack : Buff {

    public UntargetableTillAttack(AnimatorOverrideController a)
    {
        animation = a;
        text = "Stealthed\nThis unit cannot be targeted by non-area of effect attacks until its next action";
    }

    public override void CalcStats(ref Character.Stats stats, ref float hp, ref float armor)
    {
        base.CalcStats(ref stats, ref hp, ref armor);
        stats.targetable = false;
    }

    public override void OnAttackStart()
    {
        base.OnAttackStart();
        if (turnCount >= 0)
        {
            Destroy();
        }
    }

    public override void SAP()
    {
        Destroy();
    }
}
