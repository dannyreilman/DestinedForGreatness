using UnityEngine;
using System.Collections;
public class CardController : MonoBehaviour {
    public CardManager manager;
    public Character character;
    public int index = 1;

    public void SetIndex(int index)
    {
  
        this.index = index;
    }

    public void Activate()
    {
        if (character.IsAlive())
        {
            manager.GetCard(index).SetActive(true);
        }
    }

    public void Deactivate()
    {
        manager.GetCard(index).SetActive(false);
    }

}
