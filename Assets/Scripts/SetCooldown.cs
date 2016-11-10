using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetCooldown : MonoBehaviour {
    //public static Color noAttackColor = new Color(0, 0, 0);
    //public static Color attackColor = new Color(0,0,0);
    public Text[] textObjects;
    private string[] names;
    public CooldownBarHandler[] cooldownBarHandlers;

    void Start()
    {
        names = new string[4];
    }

    public void SetAttackText(int attack, string text)
    {
        names[attack - 1] = text;
    }

    public void SetAttackTexts(string[] text)
    {
        for(int i = 1; i <= 4; i++)
        {
            SetAttackText(i, text[i - 1]);
        }
    }

    public void SetCanAttack(int attack,bool b)
    {
        
        if (b)
        {
            textObjects[attack - 1].text = GetBinding(attack).ToString()[0] + ") " + names[attack - 1];
        }
        else
        {
            textObjects[attack - 1].text = '-' + ") " + names[attack - 1];
        }
        
        textObjects[attack - 1].GetComponent<AttackAnimatorManager>().set = b;
    }

    public void Cooldown(int attack, float cd)
    {
        cooldownBarHandlers[attack - 1].SetCooldown(cd);
    }

    private KeyCode GetBinding(int attack)
    {
        switch(attack)
        {
            case 1:
                return EternalBeingScript.instance.inputInst.attack1Bind;
            case 2:
                return EternalBeingScript.instance.inputInst.attack2Bind;
            case 3:
                return EternalBeingScript.instance.inputInst.attack3Bind;
            default:
                return EternalBeingScript.instance.inputInst.attack4Bind;
        }
    }

}
