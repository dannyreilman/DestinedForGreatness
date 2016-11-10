using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioPuller : MonoBehaviour {
    public string Title;
    public AudioSource source;

    public List<AudioBank> sounds;
	// Use this for initialization
	void Start () {

	}
    
    public int GetIndexByName(string name)
    {
        bool found = false;
        int index = -1;

        for(int i = 0; i < sounds.Count && !found;i++)
        {
            if(sounds[i].tag == name)
            {
                index = i;
                found = true;
            }
        }

        return index;
    }

    public void Play(int index)
    {
        if (source != null)
        {
            source.PlayOneShot(sounds[index].PullClip());
        }
        else
        {
            Debug.Log("PlayerNotFound");
        }
    }

    public void Play(string name)
    {
        int index = GetIndexByName(name);
        if (index != -1)
        {
            Play(index);
        }
        else
        {
            Debug.Log("NameNotFound");
        }
    }
}
