using System;

[Serializable]
public class StoredItem
{
    //public StoredItem(ItemData itemData, ItemType itemType)
    //{
    //    Data = itemData;
    //    Count = Data.count;
    //    ItemType = itemType;
    //}

    public StoredItem(ItemData data, ItemType itemType, AmmoTypes ammoType)
    {
        Data = data;
        ItemType = itemType;
        this.ammoType = ammoType;
    }

    public ItemData Data;
    public ItemVisual RootVisual;
    public int Count;
    public ItemType ItemType;
    public AmmoTypes ammoType;
}

public enum ItemType
{
    Weapon,
    Ammo,
    Food,
    Medicine,
    None
}