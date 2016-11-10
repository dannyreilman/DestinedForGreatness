using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BattleManagerScript : MonoBehaviour {
    public GameObject[] friendlies = new GameObject[4];
    public GameObject[] enemies = new GameObject[4];
    public GameObject Placeholder;

    public InputReciever inputReciever;
    public PrefabPuller prefabInst;

    public MapInfo info;
    char condition = 'n';
    public static bool CINEMATIC
    {
        get
        {
            return EternalBeingScript.CINEMATIC;
        }

        set
        {
            EternalBeingScript.CINEMATIC = value;
        }
    }

    private int previousHeldDirection = 0;

    // Use this for initialization
    void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            if (friendlies[i] == null)
            {
                friendlies[i] = Placeholder;
            }
            if (enemies[i] == null)
            {
                enemies[i] = Placeholder;
            }
        }
    }

    // Setting up the parties
    public void SetUpParties(CharacterInfo[] party, CharacterInfo[] encounter, MapInfo passedInfo)
    {
        friendlies = prefabInst.SpawnCharacters(party);
        enemies = prefabInst.SpawnCharacters(encounter);

        /*
        for (int i = 0; i < 4; i++)
        {
            if (friendlies[i] == null)
            {
                friendlies[i] = Placeholder;
            }
            if (enemies[i] == null)
            {
                enemies[i] = Placeholder;
            }
        }
        */

        StaggerEntrances();

        info = passedInfo;
        CorrectValues();

        CINEMATIC = true;
        StartCoroutine(Entrance());
    }

    private IEnumerator Entrance()
    {
        yield return new WaitForSeconds(0.01f);
        while (!AllReady())
        {
            yield return null;
        }
        CINEMATIC = false;
        ActivateAllReady();
        yield break;
    }

    private bool AllReady()
    {
        bool allready = true;
        foreach (GameObject c in enemies)
        {
            if (!c.GetComponent<SlotHolder>().EntranceComplete())
            {
                allready = false;
            }
        }
        foreach (GameObject c in friendlies)
        {
            if (!c.GetComponent<SlotHolder>().EntranceComplete())
            {
                allready = false;
            }
        }
        return allready;
    }

    private void StaggerEntrances()
    { 
        float[] delays = {.35f, .30f, .25f, .20f, .15f, .10f, .5f, 0f};
        List<float> delays2 = new List<float>(delays);

        int chosen = 0;

        for (int i = 0; i < 4; i++)
        {
            chosen = Random.Range(0, delays2.Count);
            friendlies[i].GetComponent<SlotHolder>().StartEntrance(delays2[chosen]);
            delays2.RemoveAt(chosen);
        }

        for (int i = 0; i < 4; i++)
        {
            chosen = Random.Range(0, delays2.Count);
            enemies[i].GetComponent<SlotHolder>().StartEntrance(delays2[chosen]);
            delays2.RemoveAt(chosen);
        }
    }

    private void ActivateAllReady()
    {
        foreach(GameObject c in friendlies)
        {
            c.GetComponent<SlotHolder>().AllReady();
        }

        foreach (GameObject c in enemies)
        {
            c.GetComponent<SlotHolder>().AllReady();
        }
    }

    public void SpawnCharacters()
    {
        for (int i = 0; i < 4; i++)
        {
            friendlies[i] = Instantiate(friendlies[i]);
            enemies[i] = Instantiate(enemies[i]);
        }
        CorrectValues();
    }

    private void CorrectValueByIndex(int index, bool side)
    {
        if (side)
        {
            switch (index)
            {
                case 1:
                    friendlies[0].GetComponent<Movement>().SetPosition(info.friendlyFront);
                    friendlies[0].transform.GetChild(0).GetComponent<Canvas>().sortingLayerName = "MidRow";
                    break;
                case 2:
                    friendlies[1].GetComponent<Movement>().SetPosition(info.friendlyTopWing);
                    friendlies[1].transform.GetChild(0).GetComponent<Canvas>().sortingLayerName = "BackRow";
                    break;
                case 3:
                    friendlies[2].GetComponent<Movement>().SetPosition(info.friendlyBottomWing);
                    friendlies[2].transform.GetChild(0).GetComponent<Canvas>().sortingLayerName = "FrontRow";
                    break;
                case 4:
                    friendlies[3].GetComponent<Movement>().SetPosition(info.friendlyBack);
                    friendlies[3].transform.GetChild(0).GetComponent<Canvas>().sortingLayerName = "MidRow";
                    break;
            }
            friendlies[index - 1].GetComponent<SlotHolder>().index = index;
            friendlies[index - 1].GetComponent<SlotHolder>().side = side;
        }
        else
        {
            switch (index)
            {
                case 1:
                    enemies[0].GetComponent<Movement>().SetPosition(info.enemyFront);
                    enemies[0].transform.GetChild(0).GetComponent<Canvas>().sortingLayerName = "MidRow";
                    break;
                case 2:
                    enemies[1].GetComponent<Movement>().SetPosition(info.enemyTopWing);
                    enemies[1].transform.GetChild(0).GetComponent<Canvas>().sortingLayerName = "BackRow";
                    break;
                case 3:
                    enemies[2].GetComponent<Movement>().SetPosition(info.enemyBottomWing);
                    enemies[2].transform.GetChild(0).GetComponent<Canvas>().sortingLayerName = "FrontRow";
                    break;
                case 4:
                    enemies[3].GetComponent<Movement>().SetPosition(info.enemyBack);
                    enemies[3].transform.GetChild(0).GetComponent<Canvas>().sortingLayerName = "MidRow";
                    break;
            }

            enemies[index - 1].GetComponent<SlotHolder>().index = index;
            enemies[index - 1].GetComponent<SlotHolder>().side = side;
            enemies[index - 1].GetComponent<SlotHolder>().Flip();
            enemies[index - 1].GetComponent<SlotHolder>().ActivateAI();
        }
    }

    private void CorrectValues()
    {
        //Set friendlies
        for(int i = 1; i <= 4; i++)
        {
            CorrectValueByIndex(i, true);
            CorrectValueByIndex(i, false);
        }
    }

    public void DoAttack(int attack, int index)
    {
        friendlies[index - 1].GetComponent<SlotHolder>().DoAttack(attack);
    }

    public bool CanAttack(int index)
    {
        return friendlies[index - 1].GetComponent<SlotHolder>().CanAttack();
    }

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

    public void DealFullAoe(Attack a, bool flying)
    {
        for(int i = Character.allCharacters.Count - 1; i >= 0; i--)
        {
            if(Character.allCharacters[i].IsAlive() && Character.allCharacters[i].flying)
            {
                Character.allCharacters[i].TakeAttack(a);
            }
        }
    }

    public void OneSideAoe(Attack a, bool side, bool flying)
    {
        for (int i = Character.allCharacters.Count - 1; i >= 0; i--)
        {
            if (Character.allCharacters[i].IsAlive() && Character.allCharacters[i].flying && Character.allCharacters[i].side == side)
            {
                Character.allCharacters[i].TakeAttack(a);
            }
        }
    }
    
    public void DealFriendlyAttack(int target, Attack a)
    {
        if (target > 0)
        {
            enemies[target - 1].GetComponent<AttackTarget>().TakeAttack(a);
        }
    }

    public void DealEnemyAttack(int target, Attack a)
    {
        if (target > 0)
        {
            friendlies[target - 1].GetComponent<AttackTarget>().TakeAttack(a);
        }
    }

    public void ReplaceCharacter(bool side, int index, CharacterInfo info)
    {
        if(side)
        {
            GameObject oldCharacter = friendlies[index - 1];
            friendlies[index - 1] = prefabInst.SpawnCharacter(info);
            CorrectValueByIndex(index,side);
            oldCharacter.GetComponent<SlotHolder>().Removed();
        }
        else
        {
            GameObject oldCharacter = enemies[index - 1];
            enemies[index - 1] = prefabInst.SpawnCharacter(info);
            CorrectValueByIndex(index, side);
            oldCharacter.GetComponent<SlotHolder>().Removed();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (condition == 'n')
        {
            //Character Control
            bool victory = true;
            bool defeat = true;
            for (int i = 0; i < 4; i++)
            {
                if (enemies[i].GetComponent<SlotHolder>().IsAlive())
                {
                    victory = false;
                }

                if (friendlies[i].GetComponent<SlotHolder>().IsAlive())
                {
                    defeat = false;
                }
            }

            if (victory)
            {
                Debug.Log("Victory");
                Exit('W');
            }
            else if (defeat)
            {
                Debug.Log("Defeat");
                Exit('L');
            }
            else
            {
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
            }
        }
    }

    private void Exit(char condition)
    {
        inputReciever.SetControlState(InputReciever.BATTLE_OVER_SCREEN);
        BattleManagerScript.CINEMATIC = true;
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

    public bool CanAttackEnemy(int index)
    {
        return enemies[index - 1].GetComponent<SlotHolder>().CanBeAttacked();
    }

    public bool CanAttackFriend(int index)
    {
        return friendlies[index - 1].GetComponent<SlotHolder>().CanBeAttacked();
    }

    public bool[] friendlyTargetArray()
    {
        bool[] returnArray = new bool[4];

        for(int i = 0; i < 4; i++)
        {
            returnArray[i] = CanAttackEnemy(i + 1);
        }

        return returnArray;
    }

    public bool[] enemyTargetArray()
    {
        bool[] returnArray = new bool[4];

        for (int i = 0; i < 4; i++)
        {
            returnArray[i] = CanAttackFriend(i + 1);
        }

        return returnArray;
    }

    public bool IsAlive(int index)
    {
        return friendlies[index - 1].GetComponent<SlotHolder>().IsAlive();
    }

}
