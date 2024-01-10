using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DinamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] protected InventorySlot_UI _slotPrefab;

    protected override void Start()
    {
        base.Start();
    }

    public override void AssignSlot(InventorySystem inventoryToDisplay)
    {
        ClearSlots();
        _slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (inventoryToDisplay == null)
            return;

        for (int i = 0; i < inventoryToDisplay.InventorySize; i++)
        {
            var uiSlot = Instantiate(_slotPrefab, transform);
            _slotDictionary.Add(uiSlot, inventoryToDisplay.InventorySlots[i]);
            uiSlot.Init(inventoryToDisplay.InventorySlots[i]);
            uiSlot.UpdateUISlot();
        }
    }

    private void ClearSlots()    // TODO: change implementation to OBJECT POOLING
    {
        foreach (var item in transform.Cast<Transform>()) // get all the children of this inventory display
        {
            Destroy(item.gameObject);
        }

        if (_slotDictionary != null)
        {
            _slotDictionary.Clear();
        }
    }

    public void RefreshDinamycInventory(InventorySystem inventoryToDisplay)
    {
        ClearSlots();
        _inventorySystem = inventoryToDisplay;
        AssignSlot(inventoryToDisplay);
    }
}
