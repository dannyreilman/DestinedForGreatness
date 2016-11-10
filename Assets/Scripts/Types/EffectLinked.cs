using UnityEngine;
using System.Collections;

public abstract class EffectLinked
{
    public GameObject effectObject;
    protected GameObject effectObjectInstance;
    public Effect linkedEffect;
    
    public void DoEffect()
    {
        if (effectObject != null && !effectObject.Equals(null))
        {
            effectObjectInstance = Object.Instantiate(effectObject);
            linkedEffect = effectObjectInstance.GetComponent<Effect>();
        }
    }
}
