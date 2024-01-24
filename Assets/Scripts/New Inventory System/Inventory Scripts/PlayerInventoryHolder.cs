using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventoryHolder : InventoryHolder
{
    public static UnityAction OnPlayerInventoryChanged;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SaveGameManager.data.PlayerInventory = new InventorySaveData(PrimaryInventorySystem);
    }

    private void OnEnable()
    {
        PlayerInputManager.OpenInventoryEvent += OpenInventory;
    }

    private void OnDisable()
    {
        PlayerInputManager.OpenInventoryEvent -= OpenInventory;
    }

    public void OpenInventory()
    {
        OnDinamicInventoryDisplayRequested?.Invoke(PrimaryInventorySystem, Offset);
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (PrimaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }

        return false;
    }

    public int RemoveFromInventory(InventoryItemData item, int amount)
    {
        foreach (var slot in PrimaryInventorySystem.InventorySlots)
        {
            if (slot.ItemData != item)
                continue;

            var actualAmount = 0;

            if (slot.StackSize > amount)
            {
                actualAmount = amount;
                slot.RemoveFromStack(actualAmount);
            }
            else
            {
                actualAmount = slot.StackSize;
                slot.RemoveFromStack(actualAmount);
                slot.ClearSlot();
            }

            OnPlayerInventoryChanged?.Invoke();

            return actualAmount;
        }

        return 0;
    }

    protected override void LoadInventory(SaveData data)
    {
        //check the save data for this specific crate inventory, and if it exists, load it 
        if (data.PlayerInventory.InventorySystem != null)
        {
            this.PrimaryInventorySystem = data.PlayerInventory.InventorySystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }
}
