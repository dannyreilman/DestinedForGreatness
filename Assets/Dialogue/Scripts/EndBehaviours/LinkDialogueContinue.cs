using UnityEngine;
using System.Collections;
using System;

namespace RPGDialogue
{
    [CreateAssetMenu()]
    public class LinkDialogueContinue: EndBehaviour
    {
        public DialogueElement nextElement;

        public override void End(bool skipping)
        {
            if (!skipping)
            {
                EternalBeingScript.instance.dialogueInst.SetContinueMarker();
                EternalBeingScript.instance.StartCoroutine(continueAfterButton());
            }
            else
            {
                EternalBeingScript.instance.dialogueInst.ContinueDialogue();
                nextElement.StartElement();
            }
        }

        private IEnumerator continueAfterButton()
        {
            bool check = false;
            do
            {
                check = EternalBeingScript.instance.dialogueInst.continueTrigger;
                if (check)
                {
                    EternalBeingScript.instance.dialogueInst.EndContinueMarker();
                    EternalBeingScript.instance.dialogueInst.ContinueDialogue();
                    nextElement.StartElement();
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }
            } while (!check);
        }
    }
}