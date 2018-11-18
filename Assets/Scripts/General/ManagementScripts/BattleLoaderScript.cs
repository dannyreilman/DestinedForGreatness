using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BattleLoaderScript : MonoBehaviour
{
    static string BATTLE_SCENE_NAME = "BattleScene"; 

    public CharacterInfo[] testparty = new CharacterInfo[4];
    public CharacterInfo[] testEncounter = new CharacterInfo[4];

    public GameObject battleManager;
    public InputReciever inputReciever;
    public PrefabPuller prefabPuller;
    private BattleManagerScript battleManagerInstance;
    
    public MapInfo baseMapInfo;

    //The party to be loaded, when the battle scene is loaded, whatever is in this array is spawned
    private CharacterInfo[] party = new CharacterInfo[4];

    //The encounter to be loaded, when the battle scene is loaded, whatever is in this array is spawned
    private CharacterInfo[] encounter = new CharacterInfo[4];

    private MapInfo mapInfo;

    void Awake()
    {
        //For Testing
        PrepareBattle(null, testparty, testEncounter);
        SceneManager.sceneLoaded += InitializeBattleManager;
    }

    public void PrepareBattle(MapInfo map, CharacterInfo[] party, CharacterInfo[] encounter)
    {
        this.party = party;
        this.encounter = encounter;
        this.mapInfo = map;
    }
    
    public void StartBattle()
    {
        SceneManager.LoadScene(BATTLE_SCENE_NAME);
    }
    
    public void StartBattle(MapInfo map, CharacterInfo[] party, CharacterInfo[] encounter)
    {
        PrepareBattle(map, party,encounter);
        StartBattle();
    }

    private void InitializeBattleManager(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == BATTLE_SCENE_NAME)
        {
            battleManagerInstance = ((GameObject)Instantiate(battleManager, transform.position, transform.rotation)).GetComponent<BattleManagerScript>();
            battleManagerInstance.inputReciever = inputReciever;
            battleManagerInstance.prefabInst = prefabPuller;
            battleManagerInstance.SetUpParties(party, encounter, mapInfo);
            inputReciever.GetComponent<InputReciever>().SetControlState(InputReciever.BATTLE_STATE);
        }
    }

    public BattleManagerScript GetBattleManagerInstance()
    {
        return battleManagerInstance;
    }

}
