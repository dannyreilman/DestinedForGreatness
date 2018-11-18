using UnityEngine;
using System.Collections;

[System.Serializable]
public class Attack : EffectLinked {
    public const int NO_DAMAGE_TYPE = 0;
    public const int TRUE_DAMAGE = 1;

    public float damage, armorIgnoreFlat, armorIgnorePercent, resistIgnoreFlat, resistIgnorePercent;
    public Vector2 knockback;
    public SlotEntity target;
    public Character characterOrigin;
    public bool crit, hurting, side, superHurting;
    public string tag = "untagged";
    public int damageType = NO_DAMAGE_TYPE;
    public AudioClip launchSound, impactSound;

    public Attack(float damage, Character origin)
    {
        this.damage = damage;
        characterOrigin = origin;
        knockback = Vector2.zero;
        armorIgnoreFlat = 0;
        armorIgnorePercent = 0;
        resistIgnoreFlat = 0;
        resistIgnorePercent = 0;
        crit = false;
        hurting = true;
        superHurting = false;
        side = origin.side;
        if (launchSound != null)
        {
            origin.soundEffects.source.PlayOneShot(launchSound);
        }
    }

    public void DoEffects(Character c)
    {
        if (impactSound != null)
        {
            c.soundEffects.source.PlayOneShot(impactSound);
        }
        if (effectObject != null)
        {
            DoEffect();
            linkedEffect.Spawn();
        }
        if (linkedEffect != null)
        {
            linkedEffect.Destroy();
        }
        Effects(c);
    }

    public bool HasDamage()
    {
        return damage > 0;
    }

    protected virtual void Effects(Character c){ }

    public void ToTrueDamage()
    {
        damageType = TRUE_DAMAGE;
        resistIgnorePercent = 1;
        armorIgnorePercent = 1;
    }

    public virtual void OnCancel() { }
    public virtual void OnMiss() { }

    //Does another attack on the same target
    public void MimicAttack(Attack mimic)
    {
        target.TakeAttack(mimic);
    }
}
