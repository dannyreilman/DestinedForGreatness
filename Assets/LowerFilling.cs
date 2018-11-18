using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowerFilling : MonoBehaviour 
{
	static float initPerBar = 50.0f;

	List<Image> children;
	int lastBarCount = 0;
	
	float perBar = initPerBar;

	
	void Awake()
	{
		children = new List<Image>();
		for(int i = 0; i < transform.childCount; ++i)
		{
			children.Add(transform.GetChild(i).GetComponent<Image>());
		}
	}

	public void SetPerBar(float perBarIn)
	{
		perBar = perBarIn;
	}

	public void UpdateBarAmt(float current, float max)
	{
		int barCount = Mathf.Max(1, Mathf.Min(Mathf.FloorToInt(max / perBar), children.Count) - 2);

		if(barCount != lastBarCount)
		{
			int i = 0;
			for(i = 0; i < barCount; ++i)
			{
				children[i].gameObject.SetActive(true);
			}

			while(i < children.Count)
			{
				children[i].gameObject.SetActive(false);
				++i;
			}
		}

		if (barCount == 0)
		{
			return;
		}

		float percent = current/max;
		float percentPer = 1.0f / barCount;
		int currentBar = 0;
		
		while(percent > percentPer)
		{
			children[currentBar].fillAmount = 1;
			percent -= percentPer;
			++currentBar;
		}

		children[currentBar].fillAmount = percent / percentPer;
		
		while(currentBar < barCount)
		{
			children[currentBar].fillAmount = 0;
			++currentBar;
		}
	}
}
