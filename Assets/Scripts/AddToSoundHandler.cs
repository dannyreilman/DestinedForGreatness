using UnityEngine;
using System.Collections;

public class AddToSoundHandler : MonoBehaviour {
    public MassAudioChange.Type type;

	// Use this for initialization
	void Start () {
        EternalBeingScript.instance.audioInst.Add(gameObject.GetComponent<AudioSource>(),type);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
