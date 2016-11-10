using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuffBar : MonoBehaviour {

    public List<Buff> buffs;
    public int buffCount;

    public List<GameObject> Rows;
    public GameObject baseRow;

    private bool needsUpdate;

    public void Awake()
    {
        buffs = new List<Buff>();
        needsUpdate = true;
    }
    
    
    public void Update()
    {
        if (needsUpdate)
        {
            buffCount = buffs.Count;
            int rowCount = (buffs.Count - 1) / 7 + 1;

            for (int i = 0; i < Rows.Count; i++)
            {
                Rows[i].SetActive(i < rowCount);
            }

            for (int i = 0; i < rowCount; i++)
            {
                Animator[] animators = new Animator[7];

                for(int j = 0; j < 7; j++)
                {
                    animators[j] = Rows[i].transform.GetChild(j).GetComponentInChildren <Animator>();
                }

                int startVal = i * 7;
                for (int j = 0; j < animators.Length && j + startVal < buffs.Count; j++)
                {
                    animators[j].runtimeAnimatorController = buffs[startVal + j].animation;
                    buffs[startVal + j].animator = animators[j];
                    Rows[i].transform.GetChild(j).GetComponent<BuffHolder>().descriptionText = buffs[startVal + j].text;
                    int count = buffs[startVal + j].count;
                    Rows[i].transform.GetChild(j).GetChild(0).GetComponent<Text>().text = count.ToString();
                    Rows[i].transform.GetChild(j).GetChild(0).gameObject.SetActive(count > 1);
                    buffs[startVal + j].pos = Rows[i].transform.GetChild(j).GetChild(0).transform.position;
                }
            }

            for (int i = 0; i < Rows.Count * 7; i++)
            {
                int row = i / 7;
                int column = i % 7;
                Rows[row].transform.GetChild(column).gameObject.SetActive(buffs.Count > i);
            }
            needsUpdate = false;
        }
    }
    

	public void AddBuff(Buff b)
    {
        if(b.animation != null)
        {
            buffs.Add(b);
        }
        needsUpdate = true;
    }
    
    public void RemoveBuff(Buff b)
    {
        buffs.Remove(b);
        needsUpdate = true;
    }
}
