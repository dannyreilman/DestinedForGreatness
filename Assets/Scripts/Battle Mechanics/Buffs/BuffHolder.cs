using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BuffHolder : MonoBehaviour
{
    public string descriptionText;
    public Text descriptionBox;

    public void activateText()
    {
        descriptionBox.text = descriptionText;
    }
}
