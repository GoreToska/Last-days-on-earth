using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [field: SerializeField] public InventoryItemData ItemData { get; private set; }
    [field: SerializeField] public int StackSize { get; private set; }

    public InventorySlot(InventoryItemData source, int amount)
    {
        ItemData = source;
        StackSize = amount;
    }

    // Create empty slot
    public InventorySlot()
    {
        ClearSlot();
    }

    public void ClearSlot()
    {
        ItemData = null;
        StackSize = -1;
    }

    public void AssignItem(InventorySlot slot)
    {
        if (ItemData == slot.ItemData)
        {
            AddToStack(slot.StackSize);
        }
        else
        {
            ItemData = slot.ItemData;
            StackSize = 0;
            AddToStack(slot.StackSize);
        }
    }

    public void UpdateInventorySlot(InventoryItemData data, int amount)
    {
        ItemData = data;
        StackSize = amount;
    }

    public bool SpaceLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.MaxStackSize - StackSize;

        return SpaceLeftInStack(amountToAdd);
    }

    public bool SpaceLeftInStack(int amountToAdd)
    {
        if (StackSize + amountToAdd <= ItemData.MaxStackSize)
            return true;
        else
            return false;
    }

    public void AddToStack(int amount)
    {
        StackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        StackSize -= amount;
    }

    public bool SplitStack(out InventorySlot splitStack)
    {
        if (StackSize <= 1)
        {
            splitStack = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(StackSize / 2);
        RemoveFromStack(halfStack);

        splitStack = new InventorySlot(ItemData, halfStack);
        
        return true;
    }
}
