using UnityEngine.EventSystems;
using System.Collections;
using System;
using UnityEngine;

public class ActivateOnClick : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public GameObject activateObject;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked");
        activateObject.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
    }
}
