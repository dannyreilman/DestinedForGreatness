using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DialogueSave
{
    public static DialogueSave current;
    public List<int> passed;
    
    public DialogueSave()
    {
        passed = new List<int>();
    }

    public bool Check(int ID)
    {
        return passed.Contains(ID);
    }

    public void Passed(int ID)
    {
        if(!passed.Contains(ID))
            passed.Add(ID);
    }
}
