using UnityEngine;
using System.Collections;

public class AttackAnimatorManager : MonoBehaviour {
    public Animator animator;
    public bool set;
	
	// Update is called once per frame
	void Update () {
        animator.SetBool("CanAttack", set);
        animator.SetTrigger("Go");
	}
}
