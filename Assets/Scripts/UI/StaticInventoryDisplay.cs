using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [field: SerializeField] public InventoryHolder InventoryHolder { get; private set; }
    [field: SerializeField] public InventorySlot_UI[] Slots { get; private set; }

    protected override void Start()
    {
        base.Start();

        if (InventoryHolder != null)
        {
            _inventorySystem = InventoryHolder.PrimaryInventorySystem;
            _inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else
        {
            Debug.LogWarning($"No inventory assigned to {this.gameObject}");
        }

        AssignSlot(_inventorySystem);
    }

    public override void AssignSlot(InventorySystem inventoryToDisplay)
    {
        _slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (Slots.Length != _inventorySystem.InventorySize)
        {
            Debug.Log($"Inventory slots out of sync on {this.gameObject}");
        }

        for (int i = 0; i < _inventorySystem.InventorySize; i++)
        {
            SlotDictionary.Add(Slots[i], _inventorySystem.InventorySlots[i]);
            Slots[i].Init(_inventorySystem.InventorySlots[i]);
        }
    }
}
