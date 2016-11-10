using UnityEngine;
using System.Collections;
using System;

/*
 * Shell to activate dialogue events from the game
 */
namespace RPGDialogue
{
    public class BeginDialogue : MonoBehaviour, Interactable
    {
        public DialogueElement entryElement;

        public void Interact()
        {
            entryElement.StartElement();
        }
    }
}