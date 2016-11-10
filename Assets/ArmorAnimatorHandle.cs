using UnityEngine;
using System.Collections;

public class ArmorAnimatorHandle : MonoBehaviour {

    public Animator animator;
    public LowerFilling lowerFilling;
	// Update is called once per frame
	void Update () {
        animator.SetFloat("fill", lowerFilling.fillAmount);
	}
}
