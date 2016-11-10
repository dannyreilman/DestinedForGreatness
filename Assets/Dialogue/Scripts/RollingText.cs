using UnityEngine;
using System.Collections;
using System;

namespace RPGDialogue
{
    [CreateAssetMenu()]
    public class RollingText : TextElement
    {
        public static float INDIVIDUAL_DELAY_SLOW = 0.1f;
        public static float INDIVIDUAL_DELAY_FAST = 0.05f;
        public static float INDIVIDUAL_DELAY
        {
            get
            {
                if(faster)
                {
                    return INDIVIDUAL_DELAY_FAST;
                }
                return INDIVIDUAL_DELAY_SLOW;
            }
        }
        public static bool faster = false;

        public string[] words;
        private int currentWordIndex;
        public override void Begin(DialogueElement containing)
        {
            base.Begin(containing);
            currentWordIndex = 0;
            CheckCase();
        }

        private void CheckCase()
        {
            if (currentWordIndex >= words.Length)
            {
                End();
            }
            else
            {
                EternalBeingScript.instance.StartCoroutine(rollWord());
            }
        }

        private IEnumerator rollWord()
        {
            string word = words[currentWordIndex];
            string initialText = EternalBeingScript.instance.dialogueInst.textBox.text;
            for(int i = 1; i <= word.Length;i++)
            {
                EternalBeingScript.instance.dialogueInst.textBox.text = initialText + word.Substring(0, i).PadRight(word.Length + 1);
                yield return new WaitForSeconds(INDIVIDUAL_DELAY);
            }
            EternalBeingScript.instance.dialogueInst.textBox.text = initialText + word + "  ";
            currentWordIndex++;
            CheckCase();
        }
    }
}
