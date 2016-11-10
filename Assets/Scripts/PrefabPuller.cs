using UnityEngine;
using System.Collections;

public class PrefabPuller : MonoBehaviour {
    //Class to hold all blank units
    public GameObject[] prefabs;
    const int WEAK_OPPONENT_INDEX  = 0;

    public GameObject SpawnCharacter(CharacterInfo info)
    {
        if (info.ID >= 0 && info.ID < prefabs.GetLength(0))
        {
            GameObject returnObject = Instantiate(prefabs[info.ID]);
            returnObject.GetComponent<Character>().SetInfo(info);
            return returnObject;
        }
        return null;
    }

    public GameObject[] SpawnCharacters(CharacterInfo[] infos)
    {
        GameObject[] returnList = new GameObject[infos.GetLength(0)];
        for(int i = 0; i < infos.GetLength(0);i++)
        {
            returnList[i] = SpawnCharacter(infos[i]);
        }
        return returnList;
    }
}
