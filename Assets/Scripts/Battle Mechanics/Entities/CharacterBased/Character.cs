using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

/**
* Class to handle functionality shared amoung all characters
*/

public abstract class Character : SingularEntity {
    //Stats Type-----------------------------------------------------------------
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
    //End stats type

    //static items
    public static List<Character> allCharacters = new List<Character>();
    public static double BASE_FILL_TIME = 30;
    public static float CINEMATIC_SLOW = 0.5f;
    public static float armorEfficiency = 0.5f;
    public static MovementType BLINK = new BlinkMovement();
    //End static items

    
    public float GetResist(int damageType)
    {
        switch(damageType)
        {
            case Attack.NO_DAMAGE_TYPE:
                return stats.defense;
        }

        return 0;
    }

    //Public inspector configurations------------------------------------------------------
    public CharacterInfo info;

    //###End inspector configurations


    //Variables---------------------------------------------------------------------
    protected bool flying = false;
    protected bool focussing = false;

    [HideInInspector]
    public bool moving;
    private int internalIndex;
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

    public override bool side
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
    protected bool ally;
    protected double fillAmount;

    //###End variables



    


    //basic stats 

    private Stats baseStats;
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
    //###End stats        

    //basic states
    protected bool alive = true;
    protected bool stunned = false;

    //End states

    


    private List<Buff> activeBuffs;
    private int buffIterator = 0;

    public List<Attack> loadedAttacks;

    public void StartEntrance(float delay)
    {
        StartCoroutine(EntranceAfterDelay(delay));
    }

    private IEnumerator EntranceAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetFloat("Offset", delay);
        Entrance();
    }
    protected abstract void ApplyPassive();

    //Objects which are found systematically on start--------------------------------------
    private AI aI;
    protected Animator animator;
    protected Animator attackAnimator;
    [HideInInspector]
    public AudioPuller voiceEffects;
    [HideInInspector]
    public AudioPuller soundEffects;
    protected CardManager cardManager;
    protected CooldownHandler cooldownHandler;
    protected CooldownBarHandler[] cdbarHandlers;
    private ResourceBarHandler resBarHandler;
    
    [HideInInspector]
    public BuffBar buffBar;

    private LowerFilling hpBar;
    private LowerFilling actionBar;
    
    protected DamageMarkerScript damageMarker;

    //[HideInInspector]
    protected Movement mover;

    protected ChangeLocationScript changeLocation;

    //For awake to work the first 4 children must be the character frame, then the voice and sound effect sources,
    // Then the Bars
    void Awake()
    {
        //Find objects
        //Debug.Log("Awake");
        aI = GetComponentInChildren<AI>();
        changeLocation = GetComponentInChildren<ChangeLocationScript>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        attackAnimator = GetComponent<Animator>();
        voiceEffects = transform.GetChild(1).GetComponent<AudioPuller>();
        soundEffects = transform.GetChild(2).GetComponent<AudioPuller>();

        mover = transform.GetChild(0).GetComponent<Movement>();

        Transform barWrapper = transform.GetChild(3).GetChild(0);

        actionBar = barWrapper.GetChild(0).GetChild(0).GetComponent<LowerFilling>();
        hpBar = barWrapper.GetChild(1).GetChild(0).GetComponent<LowerFilling>();

        allCharacters.Add(this);
        activeBuffs = new List<Buff>();
        loadedAttacks = new List<Attack>();
    }
    //###End objects which are found...

    public void AllReady()
    {
        animator.SetTrigger("All Ready");
    }

    // Use this for initialization
    protected virtual void Start ()
    {
        mover.MoveToLocation(container.GetLocation(this), BLINK);
        mover.ChangeLayer(container.GetLayer());

        //Set up states and stats
        baseStats = info.baseStats;
        baseStats.targetable = true;
        stats = baseStats;
        hp = stats.maxHp;
        armor = stats.maxArmor;

        ApplyPassive();

        actionBar.SetPerBar(1);

        if (side)
        {
            //cardManager.GetCard(index).GetComponentInParent<SetCooldown>().SetAttackTexts(info.attackTexts);
        }
    }

    protected virtual void Entrance()
    {
        animator.SetTrigger("Enter Entrance");
    }

    // Update is called once per frame
    public virtual void Update ()
    {
        if (!EternalBeingScript.CINEMATIC)
        {
            CalcStats();
            animator.SetFloat("Speed", stats.speed + UnityEngine.Random.Range(-0.01f,0.01f));
            animator.SetBool("Focussing", focussing);
            if (hp <= 0 && IsAlive())
            {
                Death();
            }

            if (!IsAlive())
            {
                hp = 0;
            }

            if (fillAmount < 1)
                fillAmount += Time.deltaTime * stats.speed / Character.BASE_FILL_TIME;

            //Ensures there is no overflow
            if (fillAmount > 1)
                fillAmount = 1;

            hp += Time.deltaTime * stats.hpRegen;
            
            hpBar.UpdateBarAmt(hp, stats.maxHp);
            actionBar.UpdateBarAmt((float)fillAmount, 1);

            if (hp > stats.maxHp)
                hp = stats.maxHp;

            if (armor > stats.maxArmor)
                armor = stats.maxArmor;


            // GetComponent<Shaking>().speed = stats.speed;

            for (buffIterator = activeBuffs.Count - 1; buffIterator >= 0; buffIterator--)
            {
                ((Buff)activeBuffs[buffIterator]).OnUpdate();
            }
            info.loadout.OnUpdate(info);
        }
        else
        {
            animator.SetFloat("Speed", stats.speed * CINEMATIC_SLOW + UnityEngine.Random.Range(-0.01f,0.01f));
        }

        //resBarHandler.UpdateNumbers(Mathf.Max((float)fillAmount, 0),armor,stats.maxArmor,hp,stats.maxHp, stunned, alive);

        //TODO: Delegates with cinematic start/stop
        mover.cinematic = EternalBeingScript.CINEMATIC;
        hpBar.transform.parent.parent.gameObject.SetActive(!EternalBeingScript.CINEMATIC);

        /** 
        if (side)
        {
            for (int i = 0; i < 4; i++)
            {
                if (info.cooldownBased[i])
                {
                    cardManager.GetCard(index).GetComponentInParent<SetCooldown>().Cooldown(i + 1, cooldownHandler.PercentLeft(i + 1));
                }
                cardManager.GetCard(index).GetComponentInParent<SetCooldown>().SetCanAttack(i + 1, CanAttack(i + 1));
            }
        }
        */
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
        buffBar.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        mover.inverted = true;
    }

    // Called on death
    protected virtual void Death()
    {
        SwapToMain();
        animator.SetBool("Dead", true);
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
        return fillAmount == 1 && IsAlive() && !moving;
    }
    protected virtual bool CanAttack2()
    {
        return fillAmount == 1 && IsAlive() && !moving;
    }
    protected virtual bool CanAttack3()
    {
        return fillAmount == 1 && IsAlive() && !moving; 
    }
    protected virtual bool CanAttack4()
    {
        return fillAmount == 1 && IsAlive() && !moving  ; 
    }

    protected virtual void TakeCost1()
    {
        TakeCostStandard();
    }
    protected virtual void TakeCost2()
    {
        TakeCostStandard();
    }
    protected virtual void TakeCost3()
    {
        TakeCostStandard();
    }
    protected virtual void TakeCost4()
    {
        TakeCostStandard();
    }

    protected virtual void TakeCost(int attack)
    {
        switch (attack)
        {
            case 1:
                TakeCost1();
                break;
            case 2:
                TakeCost2();
                break;
            case 3:
                TakeCost3();
                break;
            case 4:
                TakeCost4();
                break;
        }
    }

    //Abstract methods for all attacks, must be implemented in actual characters
    override public void DoAttack(int attackNum)
    {
        //Debug.Log("DoAttack entered with fill amount " + fillAmount);
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

    protected void DealAttack(string target, Attack a)
    {

    }

    protected void DealAssist(string target, Attack a)
    {

    }


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
                //buffBar.AddBuff(b);
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

    //TODO: implement buff last stand effects including dropping out of the air and such
    public void LastStand()
    {
        if(alive)
            Debug.Log("Last chance");
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

    protected void ActivateSAP()
    {
        /*
        if (side)
            EternalBeingScript.instance.battleInst.GetBattleManagerInstance().SAPEnemy();
        else
            EternalBeingScript.instance.battleInst.GetBattleManagerInstance().SAPFriendly();
        */
    }


    protected void FrontFirstAttack(Attack a)
    {
        SlotEntity chosenTarget = FrontFirstTargeting();

        chosenTarget.TakeAttack(a);
    }
    
    public override bool CanAttack()
    {
        return CanAttack(1) || CanAttack(2) || CanAttack(3) || CanAttack(4);
    } 

    public override void TakeAttack(Attack a)
    {
        Attack comingAttack = a;

        for (buffIterator = activeBuffs.Count-1; buffIterator >= 0; buffIterator--)
        {
            ((Buff)activeBuffs[buffIterator]).CalcIncAttack(ref comingAttack);
        }

        if (a.HasDamage() && a.hurting)
        {
            SwapToMain();
            animator.SetTrigger("Hurt");
        }

        mover.MoveToLocation(Vector2.zero, new Knockback(a.knockback, container.slotLayer), 2);
        

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
        * np past me I got dis
        * aight slightly past me I guess I trust you
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

        a.characterOrigin.OnDealtDamage(damage + armorDamage + shieldDamage,hp <= 0,a);
        OnTookDamage(damage + armorDamage + shieldDamage, hp <= 0, a);
        //damageMarker.TakeDamage(a, Mathf.FloorToInt(damage + armorDamage + shieldDamage));
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

    public override bool IsAlive()
    {
        return alive;
    }
    public bool IsFlying()
    {
        return flying;
    }
    public override bool CanBeTargeted()
    {  
        return stats.targetable & IsAlive();
    }

    public override Transform GetTransform()
    {
        return changeLocation.transform;
    }


    //**************************************************************************************
    //Basic Targeting Schemes
    protected SlotEntity OrderBasedTargeting(string[] choices)
    {
        SlotEntity chosen = null;
        bool found = false;

        for (int i = 0; i < choices.Length && (!found); i++)
        {
            SlotEntity possibleChoice;
            if(side)
                possibleChoice = Slot.ENEMY_SLOTS[choices[i]].GetTarget();
            else
                possibleChoice = Slot.ALLY_SLOTS[choices[i]].GetTarget();
            
            if (possibleChoice != null)
            {
                chosen = possibleChoice;
                found = true;
            }
        }
        return chosen;
    }

    private string[] FFTchoices = new string[] {"front", "up", "down", "back"};

    //Shorthand for generic Front-topwing-bottomwing-back attack ordering
    protected SlotEntity FrontFirstTargeting()
    {
        return OrderBasedTargeting(FFTchoices);
    }

    //******************************************************************************************
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
            if (a.target != null && a.target.CanBeTargeted())
            {
                a.target.TakeAttack(a);
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
        /*
        //Debug.Log("GetEnemyByIndex called with:" + index);
        if (side)
        {
            return EternalBeingScript.instance.battleInst.GetBattleManagerInstance().enemies[index - 1];
        }
        else
        {
            return EternalBeingScript.instance.battleInst.GetBattleManagerInstance().friendlies[index - 1];
        }
        */
        return null;
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

    public override void Hold(bool holding)
    {
        focussing = holding;
    }

    public void SwapToMain()
    {
        /**
        animator.enabled = true;
        attackAnimator.enabled = false;
        */
       animator.SetTrigger("ReturnControl");
       Debug.Log("SwapToMain");
    }

    public void SwapToAttack()
    {
       animator.SetTrigger("HandExternalControl");
       Debug.Log("SwapToAttack");
    }

    public virtual void Removed() {}

    public virtual void Added() {}
}
