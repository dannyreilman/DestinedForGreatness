using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ArmorHealthLeftScript : MonoBehaviour {
    public Text textObject;

    public void SetNumbers(int max, int current)
    {
        textObject.text = current + "/" + max;
    }

    public void ClearStats()
    {
        textObject.text = " ";
    }

}
