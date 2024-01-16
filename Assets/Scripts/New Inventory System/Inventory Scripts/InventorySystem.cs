using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class InventorySystem
{
    [field: SerializeField] public List<InventorySlot> InventorySlots { get; private set; }
    public int InventorySize => InventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int size)
    {
        InventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            InventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddToInventory(InventoryItemData itemToAdd, int amount)
    {
        if (ContainsItem(itemToAdd, out List<InventorySlot> itemSlots)) // check if item exists in inventory
        {
            foreach (var slot in itemSlots)
            {
                if (slot.SpaceLeftInStack(amount))
                {
                    slot.AddToStack(amount);
                    OnInventorySlotChanged?.Invoke(slot);
                    Debug.Log("Changed");

                    return true;

                }
            }
        }

        if (HasFreeSlot(out var freeSlot))  // try to find free slot
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amount);
            OnInventorySlotChanged?.Invoke(freeSlot);
            HotbarDisplay.OnHotbarItemAdded?.Invoke(freeSlot);

            return true;
        }

        return false;
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> slots)
    {
        slots = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList();

        return slots == null ? false : true;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot == null ? false : true;
    }
}
