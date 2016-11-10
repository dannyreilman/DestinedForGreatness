using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AudioBank
{
    public string tag;
    public List<AudioClip> clips;

    public AudioBank(string tag, AudioClip[] clips)
    {
        this.clips = new List<AudioClip>();
        this.tag = tag;
        this.clips.AddRange(clips);
    }

    public AudioClip PullClip()
    {
        return clips[Random.Range(0, clips.Count)];
    }
}
