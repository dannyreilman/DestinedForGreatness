using UnityEngine;
using System.Collections;

public class EternalBeingScript : MonoBehaviour {
    public static EternalBeingScript instance = null;

    public static bool CINEMATIC
    {
        get
        {
            //Debug.Log("Cinematic called with value " + instance.cinematicInt);
            return instance.cinematicInt > 0;
        }

        set
        {
            if(value)
            {
                instance.cinematicInt++;
                //Debug.Log("Cinematic set to" + instance.cinematicInt);
            }
            else
            {
                if(instance.cinematicInt > 0)
                {
                    instance.cinematicInt--;
                    //Debug.Log("Cinematic set to" + instance.cinematicInt);
                }
                else
                {
                    instance.cinematicInt = 0;
                    //Debug.Log("Cinematic set to" + instance.cinematicInt);
                }
            }
        }
    }

    private int cinematicInt = 0;

    public InputReciever inputInst;
    public BattleLoaderScript battleInst;
    public PrefabPuller prefabInst;
    public MassAudioChange audioInst;
    public DialogueHandler dialogueInst;
    public SettingsManager settingsInst;

	// Use this for initialization
	void Awake () {
        if (EternalBeingScript.instance == null)
        {
            EternalBeingScript.instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
