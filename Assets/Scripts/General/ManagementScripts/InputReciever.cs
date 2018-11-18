using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InputReciever: MonoBehaviour {

    public GameObject battleLoader;
    public int controlState;
    
    public const int TESTING_STATE = 0;
    public const int BATTLE_STATE = 1;
    public const int OVERWORLD_CASE = 2;
    public const int BATTLE_OVER_SCREEN = 3;

    public int heldDirection;
    public int mouseHeldDirection;

    private const int HELD_NONE = 0;
    
    private const int HELD_UP = 1;

    private const int HELD_RIGHT = 2;

    private const int HELD_DOWN = 3;

    private const int HELD_LEFT = 4;

    private string heldDirectionString
    {
        get
        {
            switch(heldDirection)
            {
                case HELD_UP:
                    return "up";
                case HELD_RIGHT:
                    return "front";
                case HELD_DOWN:
                    return "down";
                case HELD_LEFT:
                    return "back";
                default:
                    return "none";
            }
        }

        set
        {
            switch(value.ToLower())
            {
                case "up":
                    heldDirection = HELD_UP;
                    return; 
                case "front":
                    heldDirection = HELD_RIGHT;
                    return; 
                case "down":
                    heldDirection = HELD_DOWN;
                    return; 
                case "back":
                    heldDirection = HELD_LEFT;
                    return; 
                default:
                    heldDirection = 0;
                    return;
            }
        }
    }

    private void FixHolds()
    {
        (Slot.ALLY_SLOTS["back"] as SingularSlot).entity.Hold(heldDirection == HELD_LEFT);
        (Slot.ALLY_SLOTS["up"] as SingularSlot).entity.Hold(heldDirection == HELD_UP);
        (Slot.ALLY_SLOTS["down"] as SingularSlot).entity.Hold(heldDirection == HELD_DOWN);
        (Slot.ALLY_SLOTS["front"] as SingularSlot).entity.Hold(heldDirection == HELD_RIGHT);

    }

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
        controlState = BATTLE_STATE;
        heldDirection = HELD_NONE;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        rightHeld = Input.GetKey(rightBind);
        leftHeld = Input.GetKey(leftBind);
        upHeld = Input.GetKey(upBind);
        downHeld = Input.GetKey(downBind);
        switch (controlState)
        {
            case TESTING_STATE:
                //Behaviour for testing stat
                if (Input.GetKey(KeyCode.Space))
                {
                    Debug.Log("InputReceiver line 74");
                }
                break;

            case BATTLE_STATE:
                //Determine heldDirection
                if (!EternalBeingScript.CINEMATIC)
                {
                    switch (inputStyle)
                    {
                        case KEYBOARD_INPUT:
                            if(Input.GetKeyDown(rightBind))
                            {
                                heldDirection = HELD_RIGHT;
                            }
                            else if(Input.GetKeyDown(upBind))
                            {
                                heldDirection = HELD_UP;
                            }
                            else if(Input.GetKeyDown(downBind))
                            {
                                heldDirection = HELD_DOWN;
                            }
                            else if(Input.GetKeyDown(leftBind))
                            {
                                heldDirection = HELD_LEFT;
                            }
                            
                            switch(heldDirection)
                            {
                                case HELD_LEFT:
                                    if(!leftHeld)
                                    {
                                        if(rightHeld)
                                        {
                                            heldDirection = HELD_RIGHT;
                                        }
                                        else if(upHeld)
                                        {
                                            heldDirection = HELD_UP;
                                        }
                                        else if(downHeld)
                                        {
                                            heldDirection = HELD_DOWN;
                                        }
                                        else 
                                        {
                                            heldDirection = HELD_NONE;
                                        }
                                    }
                                break;
                                case HELD_UP:
                                    if(!upHeld)
                                    {
                                        if(rightHeld)
                                        {
                                            heldDirection = HELD_RIGHT;
                                        }
                                        else if(downHeld)
                                        {
                                            heldDirection = HELD_DOWN;
                                        }
                                        else if(leftHeld)
                                        {
                                            heldDirection = HELD_LEFT;
                                        }
                                        else
                                        {
                                            heldDirection = HELD_NONE;
                                        }
                                    }

                                break;
                                case HELD_DOWN:
                                    if(!downHeld)
                                    {  
                                        if(rightHeld)
                                        {
                                            heldDirection = HELD_RIGHT;
                                        }
                                        else if(upHeld)
                                        {
                                            heldDirection = HELD_UP;
                                        }
                                        else if(leftHeld)
                                        {
                                            heldDirection = HELD_LEFT;
                                        }
                                        else
                                        {
                                            heldDirection = HELD_NONE;
                                        }
                                    }

                                break;
                                case HELD_RIGHT:
                                    if(!rightHeld)
                                    {                                        
                                        if(upHeld)
                                        {
                                            heldDirection = HELD_UP;
                                        }
                                        else if(downHeld)
                                        {
                                            heldDirection = HELD_DOWN;
                                        }
                                        else if(leftHeld)
                                        {
                                            heldDirection = HELD_LEFT;
                                        }
                                        else
                                        {
                                            heldDirection = HELD_NONE;
                                        }
                                    }

                                break;
                            }
                            break;
                        case MOUSE_INPUT:
                            heldDirection = HELD_NONE;
                            Debug.Log("Not supported yet");
                            break;
                    }

                    if(heldDirection != HELD_NONE && !(Slot.ALLY_SLOTS[heldDirectionString] as SingularSlot).entity.IsAlive())
                    {
                        heldDirection = HELD_NONE;
                    }


                    if (heldDirection != HELD_NONE)
                    {
                        if (EternalBeingScript.GetBattleManager() != null)
                        {
                            if (Input.GetKeyDown(attack1Bind))
                            {
                                //Debug.Log("InputAttack1");
                                (Slot.ALLY_SLOTS[heldDirectionString] as SingularSlot).entity.DoAttack(1);
                            }
                            else if (Input.GetKeyDown(attack2Bind))
                            {
                                //Debug.Log("InputAttack2");
                                (Slot.ALLY_SLOTS[heldDirectionString] as SingularSlot).entity.DoAttack(2);
                            }
                            else if (Input.GetKeyDown(attack3Bind))
                            {
                                //Debug.Log("InputAttack3");
                                (Slot.ALLY_SLOTS[heldDirectionString] as SingularSlot).entity.DoAttack(3);
                            }
                            else if (Input.GetKeyDown(attack4Bind))
                            {
                                //Debug.Log("InputAttack4");
                                (Slot.ALLY_SLOTS[heldDirectionString] as SingularSlot).entity.DoAttack(4);
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
        FixHolds();
	}

    public void SetControlState(int state)
    {
        controlState = state;
    }
}
