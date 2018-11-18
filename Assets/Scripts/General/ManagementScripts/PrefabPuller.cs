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
}
