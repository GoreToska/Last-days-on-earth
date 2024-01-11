using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;
using System.IO;

public static class SaveLoad
{
    public static UnityAction OnSaveGame;
    public static UnityAction<SaveData> OnLoadGame;

    private static string _directory = "/SaveData/";
    private static string _fileName = "SaveGame.sav";

    public static bool Save(SaveData data)
    {
        OnSaveGame?.Invoke();

        string dir = Application.persistentDataPath + _directory;

        GUIUtility.systemCopyBuffer = dir;

        if (!System.IO.Directory.Exists(dir))
        {
            System.IO.Directory.CreateDirectory(dir);
        }

        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(dir + _fileName, json);

        Debug.Log("Saving game");

        return true;
    }

    public static SaveData Load()
    {
        string fullPath = Application.persistentDataPath + _directory + _fileName;
        SaveData data = new SaveData();

        if (System.IO.File.Exists(fullPath))
        {
            string json = System.IO.File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<SaveData>(json);

            OnLoadGame?.Invoke(data);
            Debug.Log("Loaded");
        }
        else
        {
            Debug.Log("Save file does not exist!");
        }

        return data;
    }

    public static void DeleteSaveData()
    {
        string fullPath = Application.persistentDataPath + _directory + _fileName;

        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }
}
