using UnityEngine;
using System.Collections;
using System;

namespace StageElement
{
    public class TestCharacter : Character
    {

        protected override void ApplyPassive()
        {
           Debug.Log("Setting Passive");
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


		public override bool CanBeTargeted()
		{
			return true;
		}

		public override void TakeAttack(Attack a)
		{
			Debug.Log(a.ToString());
		}
	}
}
