using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item/Inventory Item", fileName = "New Item")]
public class InventoryItemData : ScriptableObject
{
    [field: SerializeField] public int ID = -1;
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField, TextArea] public string Description { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int MaxStackSize { get; private set; }
    [field: SerializeField] public int CostValue { get; private set; }
    [field: SerializeField] public GameObject ItemPrefab { get; private set; }
    [field: SerializeField] public GameObject ItemModel { get; private set; }
    // TODO:
    // Weapon type?
    // item type

    public virtual void UseItem(PlayerEquipment playerEquipment)
    {
        Debug.Log("Using " + DisplayName);
    }

    public virtual void EquipItem(PlayerEquipment playerEquipment)
    {
        //HotbarDisplay.OnUsingCurrentItem += UseItem;
        Debug.Log("Equipping " + DisplayName);
    }
}
