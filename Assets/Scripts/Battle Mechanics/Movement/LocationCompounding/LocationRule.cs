using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocationRule 
{
	 public abstract void Apply(ref Vector3 position);
}
