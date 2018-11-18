using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Character
{
	private BurstHitbox attack2box;
	private static float ATTACK_2_COOLDOWN = 15;
	protected override void ApplyPassive()
	{
		SlimePassive passive = new SlimePassive();
		passive.Activate(this);

		mover.SetDefault(new LinearGlide(150f), this);
	}

	public class SlimePassive : Buff
	{
		public SlimePassive()
		{
			groupingName = "debug";
			posNeg = -1;
			text = "SlimePassive\nThis is a debug message";
		}
	}

	//All attacks are debug rn
	protected override void DoAttack1()
	{	
		animator.SetTrigger("HandExternalControl");
		SwapToAttack();
		attackAnimator.SetTrigger("Attack1");

		Attack a = new Attack(10, this);
		a.target = FrontFirstTargeting();
		PunchMovement punchyMove = new PunchMovement(200f);
		Pause wait = new Pause(0.25f);

		a.knockback = new Vector2(100, 25);

		LoadAttack(a);

		mover.MoveToTransform(a.target.GetTransform(), punchyMove);
		mover.MoveToTransform(a.target.GetTransform(), wait);
		mover.ChangeLayer(a.target.container.slotLayer);

		Debug.Log("Attack 1 used");
	}

	protected override bool CanAttack2()
	{
		return cooldownHandler.TimeLeft(2) == 0 && IsAlive();
	}

	protected override void TakeCost2()
	{
		cooldownHandler.StartCooldown(2, ATTACK_2_COOLDOWN);
	}
	protected override void DoAttack2()
	{
		attack2box.Trigger();
		Debug.Log("Attack 2 used");
	}

	protected override void DoAttack3()
	{
		Debug.Log("Attack 3 used");
	}

	protected override void DoAttack4()
	{
		Debug.Log("Attack 4 used");
	}
}

