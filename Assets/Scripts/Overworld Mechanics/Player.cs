using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    //Called in overworld to interact with objects in front of the player
    public Interactable GetInteractable()
    {
        return transform.GetComponentInChildren<Interactable>();
    }
}
