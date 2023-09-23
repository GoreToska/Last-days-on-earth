using System;

[Serializable]
public class StoredItem
{
    public StoredItem(ItemDefinition itemDefinition)
    {
        Details = itemDefinition;
    }

    public ItemDefinition Details;
    public ItemVisual RootVisual;
}