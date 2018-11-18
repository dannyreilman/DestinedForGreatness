/**
 * BattleManagerScript
 * This script is loaded in by the battleLoader. It does nothing until SetUpParties is called
 * with specifications for what characters are loaded in and what map to load in.
 * This script will pass control off to the end battle controller and once control is passed BackRow
 * to this script, will load the overworld
 */
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BattleManagerScript : MonoBehaviour 
{
    static string[] LANDING_ORDER = new string[] {"front", "up", "down", "back"};
    static string DEFAULT_LAND = "default";

    public InputReciever inputReciever;
    public PrefabPuller prefabInst;
    public MapInfo loadedInfo;
    
    private static char LOADING = 'l';
    private static char GO = 'g';

    char condition = LOADING;

    private int previousHeldDirection = 0;


    // Setting up the parties
    public void SetUpParties(CharacterInfo[] party, CharacterInfo[] encounter, MapInfo passedInfo)
    {
        //Spawn characters
        SpawnCharacters(party, encounter);
        StaggerEntrances();

        loadedInfo = passedInfo;

        EternalBeingScript.StartCinematic();
        StartCoroutine(Entrance());
    }

    private IEnumerator Entrance()
    {
        yield return new WaitForSeconds(0.01f);
        while (!AllReady())
        {
            yield return null;
        }
        EternalBeingScript.EndCinematic();
        ActivateAllReady();
        //condition = GO;
        yield break;
    }

    private bool AllReady()
    {
        foreach (Character c in Character.allCharacters)
        {
            if (!c.EntranceComplete())
            {
                return false;
            }
            
        }
        return true;
    }

    private void StaggerEntrances()
    { 
        foreach (Character c in Character.allCharacters)
        {
            c.StartEntrance(Random.Range(0f, 0.5f));
        }
    }

    private void ActivateAllReady()
    {
        foreach (Character c in Character.allCharacters)
        {
            c.AllReady();       
        }
    }

    public void SpawnCharacters(CharacterInfo[] party, CharacterInfo[] encounter)
    {
        foreach (CharacterInfo c in party)
        {
            if(c != null)
            {
                if(c.slot == null || c.slot == "none")
                {
                    bool found = false;
                    foreach(string option in LANDING_ORDER)
                    {
                        if(Slot.ALLY_SLOTS[option].PoliteMoveable())
                        {
                            Slot.ALLY_SLOTS[option].moveEntity(prefabInst.SpawnCharacter(c).GetComponent<Character>());
                            found = true;
                            break;
                        }
                    }
                    if(!found)
                    {
                        Slot.ALLY_SLOTS["default"].moveEntity(prefabInst.SpawnCharacter(c).GetComponent<Character>());
                    }
                }
                else
                {
                    Slot.ALLY_SLOTS[c.slot].moveEntity(prefabInst.SpawnCharacter(c).GetComponent<Character>());
                }
            }
        }

        foreach (CharacterInfo c in encounter)
        {
            if(c != null)
            {
                if(c.slot == null || c.slot == "none")
                {
                    bool found = false;
                    foreach(string option in LANDING_ORDER)
                    {
                        if(Slot.ENEMY_SLOTS[option].PoliteMoveable())
                        {
                            Slot.ENEMY_SLOTS[option].moveEntity(prefabInst.SpawnCharacter(c).GetComponent<Character>());
                            found = true;
                            break;
                        }
                    }
                    if(!found)
                    {
                        Slot.ENEMY_SLOTS["default"].moveEntity(prefabInst.SpawnCharacter(c).GetComponent<Character>());
                    }
                }
                else
                {
                    Slot.ENEMY_SLOTS[c.slot].moveEntity(prefabInst.SpawnCharacter(c).GetComponent<Character>());
                }
            }
        }
    }

    /* 
    //System to prevent stealth abuse
    //Pops when certain attacks find no target and instantly reveals all enemies/allies depending
    public void SAPEnemy()
    {
        
        foreach(GameObject e in enemies)
        {
            e.GetComponent<SlotHolder>().SAP();
        }
        
    }
    

    public void SAPFriendly()
    {
        
        foreach (GameObject e in friendlies)
        {
            e.GetComponent<SlotHolder>().SAP();
        }
        
    }
    */

    
    public void DealFullAoe(Attack a, bool flying)
    {
        OneSideAoe(a, true, flying);
        OneSideAoe(a, false, flying);
    }

    public void OneSideAoe(Attack a, bool side, bool flying)
    {
        foreach (Character c in Character.allCharacters)
        {
            if (c.IsAlive() && (!c.IsFlying() || flying) && c.side == side && c.CanBeTargeted())
            {
                c.TakeAttack(a);
            }
        }
    }

    
    public void ReplaceCharacter(bool side, string target, CharacterInfo info)
    {
        if(side)
        {
            Slot.ALLY_SLOTS[target].moveEntity(prefabInst.SpawnCharacter(info).GetComponent<Character>());
        }
        else
        {
            Slot.ENEMY_SLOTS[target].moveEntity(prefabInst.SpawnCharacter(info).GetComponent<Character>());
        }
        
    }

    // Update is called once per frame
    void Update()
    {        
        if (condition == GO)
        {
            //Character Control
            bool almostVictory = true;
            bool almostDefeat = true;

            string[] allMainCharacters = new string[]{"front", "up", "down", "back"};
            for (int i = 0; i < 4; i++)
            {
                if ((Slot.ALLY_SLOTS[allMainCharacters[i]] as SingularSlot).entity != null && (Slot.ALLY_SLOTS[allMainCharacters[i]] as SingularSlot).entity.IsAlive())
                {
                    almostVictory = false;
                }

                if ((Slot.ENEMY_SLOTS[allMainCharacters[i]] as SingularSlot).entity != null && (Slot.ENEMY_SLOTS[allMainCharacters[i]] as SingularSlot).entity.IsAlive())
                {
                    almostDefeat = false;
                }
            }

            //Trigger last stands
            if(almostVictory)
            {
                foreach(Character c in Character.allCharacters)
                {
                    if(c.side)
                    {
                        c.LastStand();
                    }
                }
            }

            if(almostDefeat)
            {
               foreach(Character c in Character.allCharacters)
                {
                    if(!c.side)
                    {
                        c.LastStand();
                    }
                }
            }

            //Now check for victory
            bool completeVictory = true;
            bool completeDefeat = true;

            foreach (Character c in Character.allCharacters)
            {
                Debug.Log(c);
                if (c.IsAlive())
                {
                    if(!c.side)
                        completeVictory = false;
                    else
                        completeDefeat = false;
                }
            }


            if (completeVictory)
            {
                Debug.Log("Victory");
                Exit('W');
            }
            else if (completeDefeat)
            {
                Debug.Log("Defeat");
                Exit('L');
            }
            else
            {
                /**
                bool SAPEnemies = true;
                bool SAPAllies = true;
                //SAP DETECTION
                for (int i = 0; i < 4; i++)
                {
                    if (enemies[i].GetComponent<SlotHolder>().CanBeAttacked())
                    {
                        SAPEnemies = false;
                    }
                    if (friendlies[i].GetComponent<SlotHolder>().CanBeAttacked())
                    {
                        SAPAllies = false;
                    }
                }

                if(SAPEnemies)
                {
                    SAPEnemy();
                }

                if (SAPAllies)
                {
                    SAPFriendly();
                }
                */
            }
        }
        else if(condition == LOADING)
        {
            //Debug.Log("Loading...");
        }
        
    }

    private void Exit(char condition)
    {
        inputReciever.SetControlState(InputReciever.BATTLE_OVER_SCREEN);
        EternalBeingScript.StartCinematic();
        this.condition = condition;
    }

    public void FinishExit()
    {
        if(condition != 'n')
        {
            inputReciever.SetControlState(InputReciever.TESTING_STATE);
            SceneManager.LoadScene("OverworldScene");
        }
    }

    public bool IsAlive(int index)
    {
        //return friendlies[index - 1].GetComponent<SlotHolder>().IsAlive();
        return false;
    }

}
