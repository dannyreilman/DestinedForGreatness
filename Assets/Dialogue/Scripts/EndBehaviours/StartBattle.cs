using UnityEngine;
using System.Collections;
using System;

namespace RPGDialogue
{
    [CreateAssetMenu()]
    public class StartBattle : EndBehaviour
    {
        //Allows fights to happen on arbitrary maps
        public bool overrideMap;
        public MapInfo mapForOverride;

        //Allows for special fights not with the main party
        public bool overrideParty;
        public CharacterInfo[] partyForOverride;


        public CharacterInfo[] enemies;

        public override void End(bool skipping)
        {
            EternalBeingScript.instance.dialogueInst.EndDialogue();
            //TODO: implement auto detection of party and location for map (Do this later once the overworld works)
            EternalBeingScript.instance.battleInst.LoadBattle(mapForOverride, partyForOverride, enemies);
        }
    }
}