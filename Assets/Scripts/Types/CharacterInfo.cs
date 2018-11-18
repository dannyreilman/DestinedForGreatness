using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class CharacterInfo : ScriptableObject {
    public bool alive;
    public int ID;

    //The slot to be loaded into, defaults to the standard front, up, down, back
    public string slot = null;

    public int level;
    public int[] loadoutSlotType = new int[3];
    public Loadout loadout;



    //Unchanging things

    public Character.Stats baseStats;
    public string[] attackTexts = new string[4];

    public bool[] cooldownBased = new bool[4];
}

