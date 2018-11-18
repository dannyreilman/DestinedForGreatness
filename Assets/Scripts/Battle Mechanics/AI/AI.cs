using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {
    //Super basic AI, never uses items, only randomly selects one of the four attacks and uses it
    public Character character;

    //value to determine intelligence
    private float hesitation;
    private bool working;

    private bool active;
    void Start()
    {
        hesitation = Random.Range(1f, 2f);
    }
    
	// Update is called once per frame
	public virtual void Update () {
        if(active)
        {
            if (character.index == 1)
            {
                if (character.CanAttack() & !working & !EternalBeingScript.CINEMATIC)
                    StartCoroutine(DoAttackAfterDelay());
            }
        }
	}

    public void SetActive(bool b)
    {
        active = b;
    }

    protected IEnumerator DoAttackAfterDelay()
    {
        working = true;
        yield return new WaitForSeconds(hesitation);
        character.DoAttack(ChooseAttack());
        hesitation = Random.Range(1f, 2f);
        working = false;
    }
    //Method to decide behavior of choosing attack
    protected virtual int ChooseAttack()
    {
        //TODO: Fix once I get all the attacks working
        return Random.Range(2, 3);
    }
}
