using UnityEngine;
using System.Collections;

public class CardManager : MonoBehaviour {
    public GameObject card1, card2, card3, card4;
    
    void Update()
    {
        int heldDirection = EternalBeingScript.instance.inputInst.heldDirection;
        bool cinematic = BattleManagerScript.CINEMATIC;
        card1.SetActive(heldDirection == 1 & !cinematic);
        card2.SetActive(heldDirection == 2 & !cinematic);
        card3.SetActive(heldDirection == 3 & !cinematic);
        card4.SetActive(heldDirection == 4 & !cinematic);
    }

    public GameObject GetCard(int index)
    {
        switch (index)
        {
            case 1:
                return card1;
            case 2:
                return card2;
            case 3:
                return card3;
            default:
                return card4;
        }
    }

    public void GetCooldownBarHandlers(int index)
    {

    }
}
