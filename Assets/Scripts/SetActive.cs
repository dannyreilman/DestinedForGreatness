using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetActive : MonoBehaviour {

	public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    //Also handles cinematicness
    public void Update()
    {
        gameObject.GetComponent<Image>().enabled = !EternalBeingScript.CINEMATIC;
    }
}
