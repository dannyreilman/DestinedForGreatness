using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleLoaderScript : MonoBehaviour
{
    public CharacterInfo[] testparty = new CharacterInfo[4];
    public CharacterInfo[] testEncounter = new CharacterInfo[4];

    public GameObject battleManager;
    public InputReciever inputReciever;
    public PrefabPuller prefabPuller;
    private BattleManagerScript battleManagerInstance;
    
    public MapInfo baseMapInfo;

    private CharacterInfo[] party = new CharacterInfo[4];
    private CharacterInfo[] encounter = new CharacterInfo[4];
    private MapInfo mapInfo;
	
    public void Awake()
    {
        if(Character.BATTLE_LOADER == null)
        {
            Character.BATTLE_LOADER = this;
        }
    }

    public void TestLoadBattle()
    {
        LoadBattle(baseMapInfo, testparty, testEncounter);
    }

    public void LoadBattle(MapInfo map, CharacterInfo[] party, CharacterInfo[] encounter)
    {
        this.party = party;
        this.encounter = encounter;
        this.mapInfo = map;
        SceneManager.LoadScene("BattleScene");
    }

    private void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().name == "BattleScene")
        {
            battleManagerInstance = ((GameObject)Instantiate(battleManager, transform.position, transform.rotation)).GetComponent<BattleManagerScript>();
            battleManagerInstance.inputReciever = inputReciever;
            battleManagerInstance.prefabInst = prefabPuller;
            battleManagerInstance.GetComponent<BattleManagerScript>().SetUpParties(party, encounter, mapInfo);
            inputReciever.GetComponent<InputReciever>().SetControlState(InputReciever.BATTLE_STATE);
            GameObject.FindGameObjectsWithTag("BattleBackgroundImage")[0].GetComponent<MapInfoController>().SetMapInfo(mapInfo);
        }
    }

    public BattleManagerScript GetBattleManagerInstance()
    {
        return battleManagerInstance;
    }

}
