using UnityEngine;
using System.Collections;
using System;

/**
 * A class for text elements 
 * Text elements should be called by Start and should eventually after an arbitrary delay call End
 */
namespace RPGDialogue
{
    public abstract class TextElement : ScriptableObject
    {
        protected DialogueElement containing;

        public virtual void Begin(DialogueElement containing)
        {
            this.containing = containing;
        }

        protected void End()
        {
            containing.Continue();
        }
    }
}
