using UnityEngine;
using System.Collections;

public class CinematicEffectsAdder : MonoBehaviour {
    public GameObject background;
    public ControlAspectRatio controller;
    private bool lastFrame;
    private bool thisFrame = true;

	void Update ()
    {
        thisFrame = EternalBeingScript.CINEMATIC;
        if (lastFrame && !thisFrame)
        {
            controller.StartTransitionDown();
        }
        else
        {
            if (thisFrame && !lastFrame)
            {
                controller.StartTransitionUp();
            }
        }
        lastFrame = thisFrame;
	}
}
