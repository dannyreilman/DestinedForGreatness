using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueHandler : MonoBehaviour {
    public Animator CenterBoxAnimator;
    public Text textBox;
    public GameObject continueMarker;
    public bool continueTrigger;

    public Image[] faces;
    private bool inDialogue = false;
    private bool loaded = false;

    void Start()
    {
        SaveLoad.LoadDialogue();
    }

    public void StartDialogue()
    {
        if (!inDialogue)
        {
            Debug.Log("Starting dialogue...");
            inDialogue = true;
            CenterBoxAnimator.SetBool("Active", true);
            textBox.text = "";
            EternalBeingScript.CINEMATIC = true;
        }
    }

    public void ContinueDialogue()
    {
        if(inDialogue)
        {
            textBox.text = "";
        }
    }

    public void EndDialogue()
    {
        if (inDialogue)
        {
            Debug.Log("Ending dialogue...");
            inDialogue = false;
            SaveLoad.SaveDialogue();
            CenterBoxAnimator.SetBool("Active", false);
            EternalBeingScript.CINEMATIC = false;
        }
    }

    public Text GetTextBox()
    {
        return textBox;
    }

    public void SetContinueMarker()
    {
        continueMarker.SetActive(true);
        continueMarker.GetComponent<Text>().text = EternalBeingScript.instance.inputInst.interactionBind.ToString();
    }

    public void EndContinueMarker()
    {
        continueMarker.SetActive(false);
    }
}
