using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class CharacterInfo : ScriptableObject {
    public bool alive;

    public int ID;
    public int level;
    public int[] loadoutSlotType = new int[3];
    public Loadout loadout;
}
