using UnityEngine;
using System.Collections;

public class FloatingNumberDestroyScript : MonoBehaviour {
    public GameObject mainObject;
    
    public void DestroyMain()
    {
        Object.Destroy(mainObject);
    }
}
