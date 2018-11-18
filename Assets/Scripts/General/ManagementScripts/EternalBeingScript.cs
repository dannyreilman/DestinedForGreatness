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
    }
    
    public static InputReciever GetInputReciever()
    {
        return instance.inputInst;
    }

    public static BattleLoaderScript GetBattleLoader()
    {
        return instance.battleInst;
    }
    
    public static BattleManagerScript GetBattleManager()
    {
        return instance.battleInst.GetBattleManagerInstance();
    }

    public static PrefabPuller GetPrefabPuller()
    {
        return instance.prefabInst;
    }

    public static MassAudioChange GetAudio()
    {
        return instance.audioInst;
    }

    public static DialogueHandler GetDialogue()
    {
        return instance.dialogueInst;
    }

    public static SettingsManager GetSettings()
    {
        return instance.settingsInst;
    }

    private int cinematicInt = 0;

    public InputReciever inputInst;
    public BattleLoaderScript battleInst;
    public PrefabPuller prefabInst;
    public MassAudioChange audioInst;
    public DialogueHandler dialogueInst;
    public SettingsManager settingsInst;

    public LightHandler lightInst;


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

    public static void StartCinematic()
    {
        instance.cinematicInt++;
        Debug.Log("Cinematic changed to " + instance.cinematicInt);
        instance.lightInst.brightStageLights = false;
    }

    public static void EndCinematic()
    {
        if(CINEMATIC)
        {
            instance.cinematicInt--;
            Debug.Log("Cinematic changed to " + instance.cinematicInt);
            if(!CINEMATIC)
                instance.lightInst.brightStageLights = true;
        }
        else
        {
            Debug.Log("Attempted to leave a cinematic while not in one");
        }
    }
}
