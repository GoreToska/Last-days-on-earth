using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [field: SerializeField] public InventoryHolder InventoryHolder { get; private set; }
    [field: SerializeField] public InventorySlot_UI[] Slots { get; private set; }

    private void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }

    private void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
    }

    protected override void Start()
    {
        base.Start();

        RefreshStaticDisplay();
    }

    public override void AssignSlot(InventorySystem inventoryToDisplay, int offset)
    {
        _slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        for (int i = 0; i < InventoryHolder.Offset; i++)
        {
            SlotDictionary.Add(Slots[i], _inventorySystem.InventorySlots[i]);
            Slots[i].Init(_inventorySystem.InventorySlots[i]);
        }
    }

    public void RefreshStaticDisplay()
    {
        if (InventoryHolder != null)
        {
            _inventorySystem = InventoryHolder.PrimaryInventorySystem;
            _inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else
        {
            Debug.LogWarning($"No inventory assigned to {this.gameObject}");
        }

        AssignSlot(_inventorySystem, 0);
    }
}
