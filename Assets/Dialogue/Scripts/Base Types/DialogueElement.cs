using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RPGDialogue
{
    [CreateAssetMenu()]
    /**
     * Base dialogue element that just calls each text element
     */ 
    public class DialogueElement : ScriptableObject
    {
        const float START_DELAY = 1;
        public EndBehaviour behaviour;

        public List<TextElement> elements;
        protected int currentElement;
        
        
        protected virtual void OnStart()
        {
            EternalBeingScript.instance.dialogueInst.StartDialogue();
        }

        protected virtual void OnEnd(bool skipping)
        {
            currentElement = 0;
            DialogueSave.current.Passed(GetInstanceID());
            if (behaviour.Equals(null) || behaviour == null)
            {
                EternalBeingScript.instance.dialogueInst.EndDialogue();
            }
            else
            {
                behaviour.End(skipping);
            }
        }

        public void StartElement()
        {
            OnStart();
            if (EternalBeingScript.instance.settingsInst.skipRepeatedCutscenes && DialogueSave.current.Check(GetInstanceID()))
            {
                Debug.Log("Skipping...");
                OnEnd(true);
            }
            else
            {
                EternalBeingScript.instance.StartCoroutine(StartAfterDelay(START_DELAY));
            }
        }

        private IEnumerator StartAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            currentElement = 0;
            CheckCase();
        }

        public void Continue()
        {
            currentElement++;
            CheckCase();
        }

        private void CheckCase()
        {
            if(currentElement >= elements.Count)
            {
                End();
            }
            else
            {
                elements[currentElement].Begin(this);
            }
        }

        protected void End()
        {
            OnEnd(false);
        }
    }
}
