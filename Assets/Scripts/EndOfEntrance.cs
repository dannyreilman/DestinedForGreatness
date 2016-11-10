using UnityEngine;
using System.Collections;

public class EndOfEntrance : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool("EntranceComplete", true);
    }
}
