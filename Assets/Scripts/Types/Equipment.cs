using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class Equipment : ScriptableObject
{
    public Character.Stats rawStats;
    public string text;

    public const int WEAPON_TYPE = 0;
    public const int ARMOR_TYPE = 1;
    public const int ACCESSORY_TYPE = 0;
    public int type;

    public Buff effect;
}
