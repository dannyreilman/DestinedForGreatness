using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

/**
 * Class to handle functionality shared amoung all characters
 */
public abstract class Character : MonoBehaviour, SlotHolder {
    [Serializable]
    public struct Stats
    {
        public bool targetable;

        public float maxHp;
        public float hpRegen;
        public float maxArmor;
        public float attack;
        public float speed;

        //DamageResists

        //NoTypeResist
        public float defense;

        public static Stats operator +(Stats a, Stats b)
        {
            a.targetable = a.targetable || b.targetable;
            a.maxHp += b.maxHp;
            a.maxArmor += b.maxArmor;
            a.hpRegen += b.hpRegen;
            a.attack += b.attack;
            a.speed += b.speed;

            a.defense += b.defense;

            return a;
        }
    }

    public CharacterInfo info;
    
    public float GetResist(int damageType)
    {
        switch(damageType)
        {
            case Attack.NO_DAMAGE_TYPE:
                return stats.defense;
        }

        return 0;
    }

    //static items
    public static List<Character> allCharacters;
    public static double BASE_FILL_TIME = 30;
    public static BattleLoaderScript BATTLE_LOADER;
    public static float CINEMATIC_SLOW = 0.5f;
    public static float UNSELECTED_GROUND_LEVEL = 0.25f;
    public static float SELECTED_GROUND_LEVEL = 0.5f;

    public static float armorEfficiency = 0.5f;
    public string[] attackTexts;

    public GameObject aI;
    public GameObject deathScribbles;
    public Animator animator;
    public Animator popAnimator;
    public Animator stealthAnimator;
    public AudioPuller soundEffects,voice;

    public CardManager cardManager;
    
    public List<Buff> activeBuffs;
    protected CooldownHandler cooldownHandler;
    private int buffIterator = 0;

    public bool flying;

    public bool[] cooldownBased = { false, false, false, false };
    public CooldownBarHandler[] cardManagers;
    private int internalIndex;
    private bool ally;

    public int index
    {
        get
        {
            return internalIndex;
        }

        set
        {
            internalIndex = value;
        }
    }

    public bool side
    {
        get
        {
            return ally;
        }

        set
        {
            ally = value;
        }
    }

    //basic stats 
    protected abstract void SetBaseStats();

    protected Stats baseStats;
    public Stats stats;

    private void CalcStats()
    {
        Stats currentStats = baseStats;
        foreach (Buff b in activeBuffs)
        {
            b.EarlyCalcStats(ref currentStats, ref hp, ref armor);
        }

        foreach (Buff b in activeBuffs)
        {
            b.CalcStats(ref currentStats, ref hp, ref armor);
        }

        foreach (Buff b in activeBuffs)
        {
            b.LateCalcStats(ref currentStats, ref hp, ref armor);
        }

        //info.loadout.CalcStats(ref currentStats, ref hp, ref armor, info);
        stats = currentStats;
    }

    public float hp;
    public float armor;
    public float shields = 0;

    public ArmorHealthLeftScript armorNumbers;
    public ArmorHealthLeftScript healthNumbers;
    //basic states
    private bool alive;
    public bool stunned;

    public GameObject fillingBar;
    public GameObject emptyBar;
    public GameObject stunBar;
    public BuffBar buffBar;

    protected double fillAmount;

    public GameObject armorBar;
    public GameObject hpBar;
    public ShadowZScaling shadowScaler;
    public Zphysics physics;
    public GameObject mainCanvas;
    public Movement mover;
    public DamageMarkerScript damageMarker;

    public List<Attack> loadedAttacks;

    public void StartEntrance(float delay)
    {
        StartCoroutine(EntranceAfterDelay(delay));
    }

    private IEnumerator EntranceAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Entrance();
    }

    public void AllReady()
    {
        animator.SetTrigger("All Ready");
    }

	// Use this for initialization
	protected virtual void Start ()
    {
        if(allCharacters == null || allCharacters.Equals(null))
        {
            allCharacters = new List<Character>();
        }

        allCharacters.Add(this);
        activeBuffs = new List<Buff>();
        loadedAttacks = new List<Attack>();
        flying = false;
        baseStats.targetable = true;
        SetBaseStats();
        stats = baseStats;
        alive = true;
        hp = stats.maxHp;
        armor = stats.maxArmor;
        cardManager = GameObject.FindWithTag("CardController").GetComponent<CardManager>();
        if (side)
        {
            cardManager.GetCard(index).GetComponentInParent<SetCooldown>().SetAttackTexts(attackTexts);
        }
    }

    protected virtual void Entrance()
    {
        animator.SetTrigger("Enter Entrance");
    }

	// Update is called once per frame
	public virtual void Update ()
    {
        if (!BattleManagerScript.CINEMATIC)
        {
            CalcStats();
            animator.SetFloat("Speed", stats.speed);
            stealthAnimator.SetBool("Stealth", !stats.targetable);
            if (hp <= 0 && IsAlive())
            {
                Death();
            }

            if (!IsAlive())
            {
                hp = 0;
                stealthAnimator.SetBool("Stealth", false);
            }

            if (fillAmount < 1)
                fillAmount += Time.deltaTime * stats.speed / Character.BASE_FILL_TIME;

            //Ensures there is no overflow
            if (fillAmount > 1)
                fillAmount = 1;

            hp += Time.deltaTime * stats.hpRegen;

            if (hp > stats.maxHp)
                hp = stats.maxHp;

            if (armor > stats.maxArmor)
                armor = stats.maxArmor;


            if (EternalBeingScript.instance.inputInst.heldDirection == index && side)
                physics.SetGroundLevel(SELECTED_GROUND_LEVEL);
            else
                physics.SetGroundLevel(UNSELECTED_GROUND_LEVEL);

            stunBar.SetActive(stunned);

            hpBar.GetComponent<LowerFilling>().SetNumBars((int)stats.maxHp / 50);
            armorBar.GetComponent<LowerFilling>().SetNumBars((int)stats.maxArmor / 10);


            fillingBar.GetComponent<Image>().fillAmount = Mathf.Max((float)fillAmount, 0);
            hpBar.GetComponent<LowerFilling>().SetFillAmount((float)hp / stats.maxHp);

            if (stats.maxArmor > 0)
                armorBar.GetComponent<LowerFilling>().SetFillAmount((float)armor / stats.maxArmor);
            else
                armorBar.GetComponent<LowerFilling>().SetFillAmount(0);


            GetComponent<Shaking>().speed = stats.speed;

            for (buffIterator = activeBuffs.Count - 1; buffIterator >= 0; buffIterator--)
            {
                ((Buff)activeBuffs[buffIterator]).OnUpdate();
            }
            //info.loadout.OnUpdate(info);
        }
        else
        {
            animator.SetFloat("Speed", stats.speed * CINEMATIC_SLOW);
            gameObject.GetComponent<Shaking>().speed = stats.speed * CINEMATIC_SLOW;
            stunBar.SetActive(false);
        }
        buffBar.gameObject.SetActive(!BattleManagerScript.CINEMATIC && alive);
        hpBar.SetActive(!BattleManagerScript.CINEMATIC);
        emptyBar.SetActive(!BattleManagerScript.CINEMATIC && alive);
        mover.cinematic = BattleManagerScript.CINEMATIC;

        armorNumbers.ClearStats();
        healthNumbers.ClearStats();

        if (side)
        {
            armorNumbers.SetNumbers(Mathf.CeilToInt(stats.maxArmor), Mathf.CeilToInt(armor));
            healthNumbers.SetNumbers(Mathf.CeilToInt(stats.maxHp), Mathf.CeilToInt(hp));

            for (int i = 0; i < 4; i++)
            {
                if (cooldownBased[i])
                {
                    cardManager.GetCard(index).GetComponentInParent<SetCooldown>().Cooldown(i + 1, cooldownHandler.PercentLeft(i + 1));
                }
                cardManager.GetCard(index).GetComponentInParent<SetCooldown>().SetCanAttack(i + 1, CanAttack(i + 1));
            }
        }
    }

    //Sets the AI on for enemies
    public void ActivateAI()
    {
        aI.SetActive(true);
    }

    public void SetMouseHeldDirection()
    {
        if(side)
            EternalBeingScript.instance.inputInst.mouseHeldDirection = index;
    }

    public void ResetMouseHeldDirection()
    {
        if (side)
            EternalBeingScript.instance.inputInst.mouseHeldDirection = 0;
    }

    public void Flip()
    {
        mainCanvas.transform.localScale = new Vector3(-1, 1, 1);
        shadowScaler.flip = true;
        buffBar.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        mover.inverted = true;
    }

    // Called on death
    protected virtual void Death()
    {
        deathScribbles.GetComponent<Animator>().SetTrigger("Death");
        deathScribbles.GetComponent<AudioSource>().Play();
        animator.SetBool("Dead", true);
        mover.Reset();
        alive = false;
    }

    private bool CanAttack(int attack)
    {
        switch(attack)
        {
            case 1:
                return CanAttack1();
            case 2:
                return CanAttack2();
            case 3:
                return CanAttack3();
            case 4:
                return CanAttack4();
        }
        return false;
    }

    //Methods to override for using cooldowns and other things
    protected virtual bool CanAttack1()
    {
        return fillAmount == 1 && IsAlive() && physics.grounded && physics.spinningGrounded;
    }
    protected virtual bool CanAttack2()
    {
        return fillAmount == 1 && IsAlive() && physics.grounded && physics.spinningGrounded;
    }
    protected virtual bool CanAttack3()
    {
        return fillAmount == 1 && IsAlive() && physics.grounded && physics.spinningGrounded; 
    }
    protected virtual bool CanAttack4()
    {
        return fillAmount == 1 && IsAlive() && physics.grounded && physics.spinningGrounded; 
    }

    protected virtual void TakeCost(int attack)
    {
        switch (attack)
        {
            case 1:
                TakeCostStandard();
                break;
            case 2:
                TakeCostStandard();
                break;
            case 3:
                TakeCostStandard();
                break;
            case 4:
                TakeCostStandard();
                break;
        }
    }

    //Abstract methods for all attacks, must be implemented in actual characters
    public void DoAttack(int attackNum)
    {
        if (CanAttack(attackNum))
        {
            OnAttackStart();
            switch (attackNum)
            {
                case 1:
                    DoAttack1();
                    break;
                case 2:
                    DoAttack2();
                    break;
                case 3:
                    DoAttack3();
                    break;
                case 4:
                    DoAttack4();
                    break;
            }
            OnAttackEnd(attackNum);
        }
    }

    protected abstract void DoAttack1();
    protected abstract void DoAttack2();
    protected abstract void DoAttack3();
    protected abstract void DoAttack4();

    public bool AddBuff(Buff b)
    {
        /**
         * Currently supportsgrouping and adding to buff bar
         */
        if (b.groupingName.Equals("nogroup"))
        {
            buffBar.AddBuff(b);
            activeBuffs.Add(b);
            return true;
        }
        else
        {
            bool found = false;
            foreach(Buff b2 in activeBuffs)
            {
                if(!found && b2.groupingName.Equals(b.groupingName))
                {
                    b2.IncreaseCount();
                    found = true;
                }
            }
            if (!found)
            {
                buffBar.AddBuff(b);
                activeBuffs.Add(b);
                return true;
            }
        }
        return false;
    }
    
    public void RemoveBuff(Buff b)
    {
        activeBuffs.Remove(b);
        buffBar.RemoveBuff(b);
    }

    //To be used later with buffs
    private void OnAttackStart()
    {
        for (buffIterator = activeBuffs.Count - 1; buffIterator >= 0; buffIterator--)
        {
            activeBuffs[buffIterator].OnAttackStart();
        }
    }

    //To be used later with buffs
    protected void OnAttackEnd(int attack)
    {
		for (buffIterator = activeBuffs.Count-1; buffIterator >= 0; buffIterator--)
        {
            activeBuffs[buffIterator].OnAttackEnd();
        }
        TakeCost(attack);
    }
    
    protected void TakeCostStandard()
    {
        fillAmount = 0;
    }

    protected void TakeCostCoolDown(float cooldown, int attack)
    {
        cooldownHandler.StartCooldown(attack, cooldown);
    }

    //Stealth prevention
    public void SAP()
    {
		for (buffIterator = activeBuffs.Count-1; buffIterator >= 0; buffIterator--)
        {
            activeBuffs[buffIterator].SAP();
        }
    }

    protected bool[] GetTargets()
    {
        if (side)
            return BATTLE_LOADER.GetBattleManagerInstance().friendlyTargetArray();

        return BATTLE_LOADER.GetBattleManagerInstance().enemyTargetArray();
    }

    protected bool CheckTarget(int index)
    {
        if (side)
        {
            return BATTLE_LOADER.GetBattleManagerInstance().CanAttackEnemy(index);
        }
        else
        {
            return BATTLE_LOADER.GetBattleManagerInstance().CanAttackFriend(index);
        }
    }

    protected void ActivateSAP()
    {
        if (side)
            BATTLE_LOADER.GetBattleManagerInstance().SAPEnemy();
        else
            BATTLE_LOADER.GetBattleManagerInstance().SAPFriendly();
    }

    public void DealAttack(int target, Attack a)
    {
        Attack outAttack = a;

        for (buffIterator = activeBuffs.Count - 1; buffIterator >= 0; buffIterator--)
        {
            ((Buff)activeBuffs[buffIterator]).CalcIncAttack(ref outAttack);
        }

        if (side)
        {
            BATTLE_LOADER.GetBattleManagerInstance().DealFriendlyAttack(target, outAttack);
        }
        else
        {
            BATTLE_LOADER.GetBattleManagerInstance().DealEnemyAttack(target, outAttack);
        }
    }

    protected void DealAttackToAllies(int target, Attack a)
    {
        if (!side)
        {
            BATTLE_LOADER.GetBattleManagerInstance().DealFriendlyAttack(target, a);
        }
        else
        {
            BATTLE_LOADER.GetBattleManagerInstance().DealEnemyAttack(target, a);
        }
    }

    protected void FrontFirstAttack(Attack a)
    {
        int chosenTarget = FrontFirstTargeting();

        DealAttack(chosenTarget, a);
    }
    
    public bool CanAttack()
    {
        return CanAttack(1) || CanAttack(2) || CanAttack(3) || CanAttack(4);
    } 

    public virtual void TakeAttack(Attack a)
    {
        Attack comingAttack = a;

		for (buffIterator = activeBuffs.Count-1; buffIterator >= 0; buffIterator--)
        {
            ((Buff)activeBuffs[buffIterator]).CalcIncAttack(ref comingAttack);
        }

        if (a.HasDamage() && a.hurting)
        {
            animator.SetTrigger("Hurt");
        }

        if(a.knockback > 0)
        {
            physics.Knockback(a.knockback);
        }

        if (!a.tag.Equals("NULL"))
        {
            a.DoEffects(this);
            DealDamage(a);
        }
    }

    //Calculates damage done through armor and resists
    public void DealDamage(Attack a)
    {
        float shieldDamage = 0;
        float armorDamage = 0;
        float damage = 0;
      

        /* Damage Calculation
         *
         * Summary:
         * The following section is really complicated and nuanced (sorry future me)
         * 
         * Current damage calculation is done by first calculating the reduction needed for resists,
         *      then armor. This reduced damage is then applied to shields, then armor, then health
         * 
         * Resists are percent damage reduction based on damage type, applied to ALL damage
         * Armor is a health barrier that decreases incoming damage by a flat amount based on how much armor is left
         *       (Once armor is depleted there is no more benefit)
         *
         * Shields are health buffs that are applied over armor. Shields should probably deplete over time.
         * Armor is explained above
         * Health is the main resource, you die when health reaches 0, even if some shields and armor remain
         * 
         * Crits are a way of ignoring armor and resists
         * Crits are automatically true damage and also deal double damage to armor
         * These should be used to counter the op power of stacking armor
         */

        if (a.damage > 0)
        {

            //Calculate the effective armor and resist based on reduction effects
            float effectiveArmor = armor;
            float effectiveResist = GetResist(a.damageType);

            effectiveArmor *= 1 - a.armorIgnorePercent;
            effectiveArmor -= a.armorIgnoreFlat;
            effectiveArmor *= armorEfficiency;

            effectiveResist *= 1 - a.resistIgnorePercent;
            effectiveResist -= a.resistIgnoreFlat;

            //Calculate damage
            float effectiveDamage = a.damage;

            //50 resist = 50% damage reduction
            if(effectiveResist >= 0)
            {
                effectiveDamage *= (50 / (50 + effectiveResist));
            }
            else
            {
                effectiveDamage *= 2 - (50 / (50 - effectiveResist));
            }

            if(effectiveArmor >= 0)
            {
                effectiveDamage -= effectiveArmor;
            }

            shieldDamage = Mathf.Min(shields, effectiveDamage);
            shieldDamage = Mathf.Max(shieldDamage, 0);
            effectiveDamage -= shieldDamage;


            if (a.crit)
            {
                effectiveDamage *= 2;
                armorDamage = Mathf.Min(armor, effectiveDamage);
                armorDamage = Mathf.Max(armorDamage, 0);
                effectiveDamage -= armorDamage;
                effectiveDamage /= 2;
            }
            else
            {
                armorDamage = Mathf.Min(armor, effectiveDamage);
                armorDamage = Mathf.Max(armorDamage, 0);
                effectiveDamage -= armorDamage;
            }

            damage = Mathf.Min(hp, effectiveDamage);
            damage = Mathf.Max(damage, 0);

            effectiveDamage -= damage;
        }

        hp -= damage;
        armor -=  armorDamage;
        shields -= shieldDamage;

        GetEnemyByIndex(a.origin).GetComponent<Character>().OnDealtDamage(damage + armorDamage + shieldDamage,hp <= 0,a);
        OnTookDamage(damage + armorDamage + shieldDamage, hp <= 0, a);
        damageMarker.TakeDamage(a, Mathf.FloorToInt(damage + armorDamage + shieldDamage));
    }

    public void OnDealtDamage(float damage, bool dead, Attack originalAttack)
    {
        for (buffIterator = activeBuffs.Count - 1; buffIterator >= 0; buffIterator--)
        {
            ((Buff)activeBuffs[buffIterator]).DealtDamage(damage,dead,originalAttack);
        }
    }

    public void OnTookDamage(float damage, bool dead, Attack originalAttack)
    {
        for (buffIterator = activeBuffs.Count - 1; buffIterator >= 0; buffIterator--)
        {
            ((Buff)activeBuffs[buffIterator]).TookDamage(damage, dead, originalAttack);
        }
    }

    public virtual bool IsAlive()
    {
        return alive;
    }

    public virtual bool CanBeAttacked()
    { 
        return stats.targetable & IsAlive();
    }

    //**************************************************************************************
    //Basic Targeting Schemes
    //Shorthand for generic Front-topwing-bottomwing-back attack ordering
    protected int FrontFirstTargeting()
    {
        bool[] targets = GetTargets();
        bool found = false;
        int chosenTarget = -2;

        for (int i = 0; i < 4 && !found; i++)
        {
            if (targets[i])
            {
                found = true;
                chosenTarget = i;
            }
        }
        return (chosenTarget + 1);
    }
     
    public virtual bool EntranceComplete()
    {
        return animator.GetBool("EntranceComplete");
    }

    //Function for cancelable attacks, load an attack then call the UseAttack method from the animator object when the animation is complete
    public void LoadAttack(Attack a)
    {
        loadedAttacks.Add(a);
    }

    public void CancelLoadedAttacks()
    {
        foreach (Attack a in loadedAttacks)
        {
            a.OnCancel();
        }
        loadedAttacks.Clear();
    }

    public void CancelLoadedAttack(Attack a)
    {
        a.OnCancel();
        loadedAttacks.Remove(a);
    }

    public void DoLoadedAttacks()
    {
        foreach (Attack a in loadedAttacks)
        {
            if (a.target != -1 && CheckTarget(a.target))
            {
                DealAttack(a.target, a);
            }
            else
            {
                a.OnMiss();
            }
        }
        loadedAttacks.Clear();
    }

    public GameObject GetEnemyByIndex(int index)
    {
        //Debug.Log("GetEnemyByIndex called with:" + index);
        if (side)
        {
            return BATTLE_LOADER.GetBattleManagerInstance().enemies[index - 1];
        }
        else
        {
            return BATTLE_LOADER.GetBattleManagerInstance().friendlies[index - 1];
        }
    }

    public void DestroyCharacterPermanantly()
    {
        allCharacters.Remove(this);
        Destroy(gameObject);
    }

    public void SetInfo(CharacterInfo info)
    {
        this.info = info;
    }

    public virtual void Removed() {}

    public virtual void Added() {}
}