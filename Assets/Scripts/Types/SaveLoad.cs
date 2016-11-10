using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad
{
    public const string SAVES_FOLDER = "/saves";
    public static string DIALOGUE_FILENAME = "/dialogueSave.gd";
    
    public static void SaveDialogue()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + SAVES_FOLDER + DIALOGUE_FILENAME);
        bf.Serialize(file, DialogueSave.current);
        file.Close();
    }

    public static void LoadDialogue()
    {
        if (File.Exists(Application.persistentDataPath + SAVES_FOLDER + DIALOGUE_FILENAME))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + SAVES_FOLDER + DIALOGUE_FILENAME, FileMode.Open);
            DialogueSave.current = (DialogueSave)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            DialogueSave.current = new DialogueSave();
        }
    }
}
