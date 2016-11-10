using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu()]
public class Loadout : ScriptableObject
{
    public Equipment[] equipment = new Equipment[3];

    public void CalcStats(ref Character.Stats currentStats, ref float hp, ref float armor, CharacterInfo info)
    {
        for (int i = 0; i < 3; i++)
        {
            if (equipment[i] != null && equipment[i].type == info.loadoutSlotType[i])
            {
                currentStats += equipment[i].rawStats;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (equipment[i] != null && equipment[i].type == info.loadoutSlotType[i])
            {
                equipment[i].effect.CalcStats(ref currentStats, ref hp, ref armor);
            }
        }
    }

    public void OnUpdate(CharacterInfo info)
    {
        for (int i = 0; i < 3; i++)
        {
            if (equipment[i] != null && equipment[i].type == info.loadoutSlotType[i])
            {
                equipment[i].effect.OnUpdate();
            }
        }
    }
}
