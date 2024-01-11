using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] protected int _backpackSize;
    [SerializeField] protected InventorySystem _secondaryInventorySystem;

    public InventorySystem SecondaryInventorySystem => _secondaryInventorySystem;

    public static UnityAction<InventorySystem> OnPlayerBackpackDisplayRequested;


    protected override void Awake()
    {
        base.Awake();

        _secondaryInventorySystem = new InventorySystem(_backpackSize);
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
        OnPlayerBackpackDisplayRequested?.Invoke(SecondaryInventorySystem);
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (PrimaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }
        else if (SecondaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }

        return false;
    }
}
