using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public List<string> CollectedItems = new List<string>();
    public SerializableDictionary<string, ItemPickUpSaveData> ActiveItems;
    public SerializableDictionary<string, InventorySaveData> CrateDictionary;
    public InventorySaveData PlayerInventory;

    public SaveData()
    {
        CollectedItems = new List<string>();
        ActiveItems = new SerializableDictionary<string, ItemPickUpSaveData>();
        CrateDictionary = new SerializableDictionary<string, InventorySaveData>();
        PlayerInventory = new InventorySaveData();
    }
}
