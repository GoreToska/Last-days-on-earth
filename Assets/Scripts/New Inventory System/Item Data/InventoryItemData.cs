using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Item", fileName = "New Item")]
public class InventoryItemData : ScriptableObject
{
    [field: SerializeField] public int ID = -1;
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField, TextArea] public string Description { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int MaxStackSize { get; private set; }

    // TODO:
    // Weapon type?
    // item type
}
