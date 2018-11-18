using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MassAudioChange : MonoBehaviour {

    public enum Type
    {
        soundEffect, music, voice
    }

    [Range(0.0f, 1.0f)]
    public float masterVolume;
    [Range(0.0f,1.0f)]
    public float soundEffectVolume;
    [Range(0.0f, 1.0f)]
    public float voiceVolume;
    [Range(0.0f, 1.0f)]
    public float musicVolume;

    public List<AudioSource> soundEffects, voices, music;

    public void Add(AudioSource a, Type t)
    {
        switch(t)
        {
            case Type.music:
                music.Add(a);
                break;
            case Type.soundEffect:
                soundEffects.Add(a);
                break;
            case Type.voice:
                voices.Add(a);
                break;
        }
    }

    // Use this for initialization
    void Awake () {
        soundEffects = new List<AudioSource>();
        voices = new List<AudioSource>();
        music = new List<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
	    
	}

    public void UpdateAudioSources()
    {
        foreach(AudioSource a in soundEffects)
        {
            a.volume = masterVolume * soundEffectVolume;
        }

        foreach (AudioSource a in voices)
        {
            a.volume = masterVolume * voiceVolume;
        }

        foreach (AudioSource a in music)
        {
            a.volume = masterVolume * musicVolume;
        }
    }
}
