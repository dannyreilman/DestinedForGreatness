using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageMarkerScript : MonoBehaviour
{
    public Color[] damageTypeColors;
    public float minTime;
    public GameObject damageMarker;
    public Transform spawnLocation;
    public float variationRange;
    float staggerTime = 0;

    void Update()
    {
        staggerTime -= Time.deltaTime;
        if(staggerTime < 0)
        {
            staggerTime = 0;
        }
    }

    public void TakeDamage(Attack attack, int damage)
    {
        StartCoroutine(TakeDamageInternal(attack, damage));
    }

    private IEnumerator TakeDamageInternal(Attack attack, int damage)
    {
        
        while(staggerTime > .00000000001)
        {
            yield return new WaitForEndOfFrame();
        }
        
        if (attack.HasDamage())
        {
            float variation = Random.Range(-variationRange, variationRange);
            GameObject instance = (GameObject)Instantiate(damageMarker, spawnLocation.position + new Vector3(variation, 0), Quaternion.identity);
            Text textObject = instance.transform.GetChild(0).GetComponentInChildren<Text>();
            GameObject sprite = instance.transform.GetChild(0).GetChild(1).gameObject;
            Animator animator = instance.transform.GetChild(0).GetComponentInChildren<Animator>();

            textObject.color = damageTypeColors[attack.damageType];
            sprite.SetActive(attack.crit);
            textObject.text = damage.ToString();

            if (attack.hurting)
            {
                if (attack.superHurting)
                {
                    animator.SetTrigger("Hard");
                }
                else
                {
                    animator.SetTrigger("Medium");
                }
            }
            else
            {
                animator.SetTrigger("Weak");
            }
        }

        staggerTime = minTime;
    }

}
