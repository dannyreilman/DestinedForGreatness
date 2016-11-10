using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InputReciever: MonoBehaviour {
    //TODO: REMOVE THIS AFTER TESTING
    public bool cinematic;

    public Player player;

    public GameObject battleLoader;
    public int controlState;
    public int heldDirection;
    public int mouseHeldDirection;

    public const int TESTING_STATE = 0;
    public const int BATTLE_STATE = 1;
    public const int OVERWORLD_CASE = 2;
    public const int BATTLE_OVER_SCREEN = 3;

    public int inputStyle;
    public const int KEYBOARD_INPUT = 0;
    public const int MOUSE_INPUT = 1; 
    //Keybindings
    public KeyCode upBind, downBind, rightBind, leftBind, interactionBind;
    public KeyCode attack1Bind, attack2Bind, attack3Bind, attack4Bind;

    //allocating memory for update
    public bool rightHeld, upHeld, downHeld, leftHeld;

	// Use this for initialization
	void Start ()
    {
        controlState = OVERWORLD_CASE;
        heldDirection = 0;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        cinematic = EternalBeingScript.CINEMATIC;

        switch (controlState)
        {
            case OVERWORLD_CASE:
                if (!EternalBeingScript.CINEMATIC)
                {
                    rightHeld = Input.GetKey(rightBind);
                    leftHeld = Input.GetKey(leftBind);
                    upHeld = Input.GetKey(upBind);
                    downHeld = Input.GetKey(downBind);

                    if (Input.GetKeyDown(interactionBind))
                    {
                        Interactable found = player.GetInteractable();
                        if(!found.Equals(null) && found != null)
                        {
                            found.Interact();
                        }
                    }
                }
                else
                {
                    rightHeld = false;
                    upHeld = false;
                    downHeld = false;
                    leftHeld = false;

                    EternalBeingScript.instance.dialogueInst.continueTrigger = Input.GetKeyDown(interactionBind);
                }
                break;

            case TESTING_STATE:
                //Behaviour for testing stat
                if (Input.GetKey(KeyCode.Space))
                {
                    battleLoader.GetComponent<BattleLoaderScript>().TestLoadBattle();
                }
                break;

            case BATTLE_STATE:
                //Determine heldDirection
                if (!BattleManagerScript.CINEMATIC)
                {
                    switch (inputStyle)
                    {
                        case KEYBOARD_INPUT:
                            rightHeld = Input.GetKey(rightBind);
                            upHeld = Input.GetKey(upBind);
                            downHeld = Input.GetKey(downBind);
                            leftHeld = Input.GetKey(leftBind);

                            switch (heldDirection)
                            {
                                case 1:
                                    if (!rightHeld)
                                    {
                                        if (upHeld)
                                            heldDirection = 2;
                                        else if (downHeld)
                                            heldDirection = 3;
                                        else if (leftHeld)
                                            heldDirection = 4;
                                        else
                                            heldDirection = 0;
                                    }
                                    break;
                                case 2:
                                    if (!upHeld)
                                    {
                                        if (rightHeld)
                                            heldDirection = 1;
                                        else if (downHeld)
                                            heldDirection = 3;
                                        else if (leftHeld)
                                            heldDirection = 4;
                                        else
                                            heldDirection = 0;
                                    }
                                    break;
                                case 3:
                                    if (!downHeld)
                                    {
                                        if (rightHeld)
                                            heldDirection = 1;
                                        else if (upHeld)
                                            heldDirection = 2;
                                        else if (leftHeld)
                                            heldDirection = 4;
                                        else
                                            heldDirection = 0;
                                    }
                                    break;
                                case 4:
                                    if (!leftHeld)
                                    {
                                        if (rightHeld)
                                            heldDirection = 1;
                                        else if (upHeld)
                                            heldDirection = 2;
                                        else if (downHeld)
                                            heldDirection = 3;
                                        else
                                            heldDirection = 0;
                                    }
                                    break;
                                default:
                                    if (rightHeld)
                                        heldDirection = 1;
                                    else if (upHeld)
                                        heldDirection = 2;
                                    else if (downHeld)
                                        heldDirection = 3;
                                    else if (leftHeld)
                                        heldDirection = 4;
                                    else
                                        heldDirection = 0;
                                    break;
                            }
                            break;
                        case MOUSE_INPUT:
                            heldDirection = mouseHeldDirection;
                            break;
                    }

                    if(heldDirection != 0 && !battleLoader.GetComponent<BattleLoaderScript>().GetBattleManagerInstance().GetComponent<BattleManagerScript>().IsAlive(heldDirection))
                    {
                        heldDirection = 0;
                    }

                    if (heldDirection != 0)
                    {
                        if (battleLoader.GetComponent<BattleLoaderScript>().GetBattleManagerInstance().GetComponent<BattleManagerScript>() != null)
                        {
                            if (Input.GetKeyDown(attack1Bind))
                            {
                                battleLoader.GetComponent<BattleLoaderScript>().GetBattleManagerInstance().GetComponent<BattleManagerScript>()
                                    .DoAttack(1, heldDirection);
                            }
                            else if (Input.GetKeyDown(attack2Bind))
                            {
                                battleLoader.GetComponent<BattleLoaderScript>().GetBattleManagerInstance().GetComponent<BattleManagerScript>()
                                    .DoAttack(2, heldDirection);
                            }
                            else if (Input.GetKeyDown(attack3Bind))
                            {
                                battleLoader.GetComponent<BattleLoaderScript>().GetBattleManagerInstance().GetComponent<BattleManagerScript>()
                                    .DoAttack(3, heldDirection);
                            }
                            else if (Input.GetKeyDown(attack4Bind))
                            {
                                battleLoader.GetComponent<BattleLoaderScript>().GetBattleManagerInstance().GetComponent<BattleManagerScript>()
                                    .DoAttack(4, heldDirection);
                            }
                        }
                    }
                }
                break;
            case BATTLE_OVER_SCREEN:
                heldDirection = 0;
                if (Input.GetKey(KeyCode.Space))
                {
                    battleLoader.GetComponent<BattleLoaderScript>().GetBattleManagerInstance().FinishExit();
                }
                break;
        }
	}

    public void SetControlState(int state)
    {
        controlState = state;
    }
}
