using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CooldownBarHandler : MonoBehaviour {
    public Animator animator;

	public void SetCooldown(float cd)
    {
        gameObject.GetComponent<Image>().fillAmount = cd;
        animator.SetFloat("fillAmount", cd);
    }
}
