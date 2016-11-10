using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TagList
{
    public List<String> possibleTags;
    public List<String> currentTags;

    public TagList(List<String> e)
    {
        possibleTags = e;
    }

    public void AddTag(String tag)
    {
        if(!currentTags.Contains(tag))
        {
            currentTags.Add(tag);
        }
    }

    public void RemoveTag(String tag)
    {
        currentTags.Remove(tag);
    }
    
    public bool Check(String tag)
    {
        return currentTags.Contains(tag);
    }

}
