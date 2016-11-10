using UnityEngine;
using System.Collections;

public class CancelableAttackHandler : MonoBehaviour {
    public Character character;
    public bool resetOnIdle = false;

    public void FinishAttack()
    {
        character.DoLoadedAttacks();
    }

    public void CancelAttack()
    {
        character.CancelLoadedAttacks();
    }

    public void IdleStart()
    {
        if(resetOnIdle)
        {
            Reset();
        }
    }

    public void Reset()
    {
        character.mover.Reset();
    }
}
