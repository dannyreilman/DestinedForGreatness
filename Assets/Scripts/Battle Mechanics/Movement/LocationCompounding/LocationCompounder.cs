using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script should be used to set the character position
 * This script allows for an infinite number of offsets without needing a shitton of empty gameobjects
 */

public class LocationCompounder : MonoBehaviour 
{

	private List<LocationRule> rules;
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 currentPosition = transform.position;
		foreach(LocationRule rule in rules)
		{
			rule.Apply(ref currentPosition);
		}
		transform.position = currentPosition;
	}

	public void AddRule(LocationRule r)
	{
		rules.Add(r);
	}

	public void RemoveRule(LocationRule r)
	{
		rules.Remove(r);
	}
}
