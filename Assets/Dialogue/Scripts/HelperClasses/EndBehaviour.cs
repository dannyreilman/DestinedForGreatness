using UnityEngine;
using System.Collections;

namespace RPGDialogue
{
    public abstract class EndBehaviour : ScriptableObject
    {
        public abstract void End(bool skipping);
    }
}
