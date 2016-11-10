using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LowerFilling : MonoBehaviour {
    public float fillAmount;
    public int numBars;
    public List<GameObject> fillBars;
    public GameObject firstBar;
    public GameObject lastBar;

    public void Awake()
    {
        int numMidRows = transform.childCount;
        fillBars = new List<GameObject>();

        for(int i = 0; i < numMidRows; i++)
        {
            if (transform.GetChild(i).gameObject.name == "MidBar")
            {
                fillBars.Add(transform.GetChild(i).gameObject);
            }
        }

    }

	public void SetFillAmount(float f)
    {
        if (fillAmount != f)
        {
            fillAmount = f;
            UpdateFillAmount();
        }
    }

    public void SetNumBars(int n)
    {
        numBars = n;
        for (int i = 0; i < fillBars.Count; i++)
        {
            fillBars[i].SetActive(i < numBars);
        }
    }

    private void UpdateFillAmount()
    {
        float actualNumBars = numBars + 2;
        float cumAmount = fillAmount * actualNumBars;
        for (int barNum = 0; barNum < actualNumBars; barNum++)
        {
            float fillingUp = Mathf.Min(cumAmount, 1);

            if (barNum == 0)
            {
                firstBar.GetComponent<Image>().fillAmount = fillingUp;
            }
            else if (barNum == actualNumBars - 1)
            {
                lastBar.GetComponent<Image>().fillAmount = fillingUp;
            }
            else
            {
                fillBars[barNum - 1].GetComponent<Image>().fillAmount = fillingUp;
            }
            cumAmount -= fillingUp;
        }
    }    
    
}
