using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Epidemic/Data/Item")]
public class ItemDefinition : ScriptableObject
{
    public string ID = Guid.NewGuid().ToString();
    [TextArea]
    public string friendlyName;
    [TextArea]
    public string description;
    public int sellPrice;
    public float wheight;
    public Sprite icon;
    public Dimensions slotDimension;
}

[Serializable]
public struct Dimensions // the number of hight and width slots the item should take up on inventory ui
{
    public int Height; // minimum 1
    public int Width; // minimum 1
}