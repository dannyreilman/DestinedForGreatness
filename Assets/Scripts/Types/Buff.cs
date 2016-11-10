using UnityEngine;
using UnityEditor.Animations;
using System;
using System.Collections.Generic;

[Serializable]
public class Buff : EffectLinked
{
    //tags
    public AnimatorOverrideController animation = null;
    public Animator animator = null;
    public string text = " ";
    public string groupingName = "nogroup";

    public Vector2 pos;
    //TAGS
    public int posNeg = 0;
    public int count = 0;

    public virtual void IncreaseCount()
    {
        count++;
        if (animator != null)
            animator.SetTrigger("IncreaseCount");
    }

    public virtual void DecreaseCount()
    {
        count--;
        target.buffBar.needsUpdate = true;
    }

    protected int turnCount;
    protected float timePassed;

    public Character target;
     
    //Stealth abuse protection
    public virtual void SAP() {}
    
    protected virtual void OnActivation()
    {
        if(effectObject != null)
        {
            DoEffect();
            linkedEffect.Spawn();
        }
        IncreaseCount();
    }

    public virtual void OnUpdate()
    {
        timePassed += Time.deltaTime;
    }

    public virtual void OnAttackReady() {}
    public virtual void OnAttackStart() {}
    public virtual void OnAttackEnd()
    {
        turnCount++;
    }
    public virtual void DealtDamage(float damage, bool dead,Attack originalAttack) { }
    public virtual void TookDamage(float damage, bool dead, Attack originalAttack) { }

    public virtual void EarlyCalcStats(ref Character.Stats stats, ref float hp, ref float armor) { }
    public virtual void CalcStats(ref Character.Stats stats, ref float hp, ref float armor) { }
    public virtual void LateCalcStats(ref Character.Stats stats, ref float hp, ref float armor) { }


    public virtual void CalcIncAttack(ref Attack a) { }
    public virtual void CalcOutAttack(ref Attack a) { }


    protected virtual void OnDestroyed()
    {
        if (linkedEffect != null)
        {
            linkedEffect.Destroy();
        }
    }

    public Buff() {}

    public Buff(Character target) : this()
    {
        Activate(target);
    }

    public void Activate(Character target)
    {
        pos = new Vector2(0, 0);
        this.target = target;
        if (target.AddBuff(this))
        {
            turnCount = 0;
            DoEffect();
            OnActivation();
        }
        else
            Destroy();
    }
    
    public void Destroy()
    {
        OnDestroyed();
        DecreaseCount();
        if (count < 1)
            target.RemoveBuff(this);
    }
    public void DestroyAll()
    {
        OnDestroyed();
        count = 0;
        target.RemoveBuff(this);
    }

    //helpful methods
    protected float KeepPercent(float amt, float max1, float max2)
    {
        float percent = amt / max1;
        return max2 * percent;
    }
    
}
