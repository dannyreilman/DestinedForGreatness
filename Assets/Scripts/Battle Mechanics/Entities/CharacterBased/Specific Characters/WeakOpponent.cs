using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace StageElement
{
    public class WeakOpponent : Character
    {
        public AnimatorOverrideController passiveBuffSymbol, chipAwaySymbol, stealthSymbol, cowerSymbol;
        public int greatHeroTransformation;
        public RecklessConfidence passive;
        public float chipAwayKnockback;


        //Sets base stats and passive
        //Passive - Reckless Confidence - Speeds up at lower health/slows down at higher health
        protected override void ApplyPassive()
        {
            
            passive = new RecklessConfidence(passiveBuffSymbol);
            passive.Activate(this);
        }

        protected override void Entrance()
        {
            base.Entrance();
        }

        [System.Serializable]
        public class RecklessConfidence : Buff
        {
            //note this value is total swing and not speed gained at 0 hp
            private float speedAmt = 10;

            public RecklessConfidence(AnimatorOverrideController a)
            {
                animation = a;
                text = "Reckless Confidence\nThis unit has increased speed per missing health";
            }
            
            public override void CalcStats(ref Stats stats, ref float hp, ref float armor)
            {
                float hpPercent = hp / stats.maxHp;
                if (hpPercent > .5)
                {
                    posNeg = -1;
                }
                else
                {
                    posNeg = 1;
                }
                    stats.speed = stats.speed + (hpPercent - .5f) * -1 * speedAmt;
            }
        }

        //Take Risks
        //This attack will replace WO with GREAT HERO, a much stronger character
        //but only during certain conditions (see notes)
        protected override void DoAttack1()
        {
            /*
            int negBuffSum = 0;

            foreach (Buff b in activeBuffs)
            {
                if (b.posNeg == -1)
                {
                    negBuffSum++;
                }
            }

            if (hp > 0)
            {
                float odds = 4 * (stats.maxHp / hp) + Mathf.Pow(negBuffSum, 2) + Mathf.Pow(info.level,2);
                if(Random.Range(1,100) <= odds)
                {
                    info.ID = greatHeroTransformation;
                    BATTLE_LOADER.GetBattleManagerInstance().ReplaceCharacter(side,index, info);
                }
            }
            */
           //For testing, this attack is complicated
           Debug.Log("Attack 1 used");
        }

        //Chip Away
        //
        protected override void DoAttack2()
        {
            animator.SetTrigger("Attack");
            float onHitDamage = .5f * stats.attack + 1;
            float initialDamage = .5f * stats.attack + 1;
            BuffCarrier passingAttack = new BuffCarrier(initialDamage, this, new ChippedAway(chipAwaySymbol, onHitDamage));
            
            passingAttack.target = FrontFirstTargeting();
            
            LoadAttack(passingAttack);
        }

        private class ChippedAway : Buff
        {
            private Attack onHitAttack;
            public ChippedAway(AnimatorOverrideController a, float onHitDamage)
            {
                animation = a;
                groupingName = "chipaway";
                posNeg = -1;
                text = "Chipped Armor\nThis unit's armor is becoming increasingly compromised";
            }

            protected override void OnActivation()
            {
                base.OnActivation();
                onHitAttack = new Attack(1, target);
                onHitAttack.ToTrueDamage();
                onHitAttack.tag = "ChipAwayOnHit";
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
                if (target.armor <= 0)
                {
                    DestroyAll();
                }
            }

            public override void TookDamage(float damage, bool dead, Attack originalAttack)
            {
                base.TookDamage(damage, dead, originalAttack);
                if (target.armor <= 0)
                {
                    DestroyAll();
                }
                else
                {
                    onHitAttack.damage = count;
                    if(originalAttack.tag != "ChipAwayOnHit")
                        originalAttack.MimicAttack(onHitAttack);
                }
            }
        }

        //Shy Away
        //Makes this character untargetable until next attack
        protected override void DoAttack3()
        {
            new UntargetableTillAttack(stealthSymbol).Activate(this);
        }

        //Cower
        //Reduces all character's speed
        protected override void DoAttack4()
        {
            EternalBeingScript.GetBattleManager().DealFullAoe(new BuffCarrier(0, this, new Attack4Effects(cowerSymbol)), true);
        }

        private class Attack4Effects : Buff
        {
            List<float> durations;
            public Attack4Effects(AnimatorOverrideController a)
            {
                animation = a;
                groupingName = "lowmorale";
                text = "Lowered Morale\nThis unit has reduced speed";
                durations = new List<float>();
                posNeg = -1;
            }

            public override void IncreaseCount()
            {
                base.IncreaseCount();
                durations.Add(10f);
            }

            public override void CalcStats(ref Stats stats, ref float hp, ref float armor)
            {
                stats.speed = stats.speed / Mathf.Pow(1.5f, count);
            }

            public override void OnUpdate()
            {
                base.OnUpdate();

                for (int i = durations.Count - 1; i >= 0; i--)
                {
                    durations[i] -= Time.deltaTime;

                    if(durations[i] <= 0)
                    {
                        durations.RemoveAt(i);
                        Destroy();
                    }
                }
            }

        }

    }
}