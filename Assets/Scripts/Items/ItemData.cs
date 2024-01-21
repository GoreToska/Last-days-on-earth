using System;
using UnityEngine;

public class ItemData : ScriptableObject
{
    [TextArea]
    public string name;
    [TextArea] 
    public string description;
    public float wheight;
    [SerializeField] public int sellPrice;
    [SerializeField] public Sprite image;
    public ItemCharacteristic itemCharacteristics;
    public int count;

    [Serializable]
    public struct ItemCharacteristic // the number of hight and width slots the item should take up on inventory ui
    {
        public int Height; // minimum 1
        public int Width; // minimum 1
        public GameObject itemPrefab;
    }
}
