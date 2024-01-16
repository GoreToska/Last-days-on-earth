using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HotbarDisplay : StaticInventoryDisplay
{
    [field: SerializeField] public int CurrentIndex { get; private set; }
    public static UnityAction<InventoryItemData> OnItemEquip = delegate { };
    public static UnityAction<InventorySlot> OnHotbarItemAdded = delegate { };
    public static UnityAction<InventorySlot> OnCurrentSlotItemAdded = delegate { };

    private int _maxIndex = 4;
    private int _minIndex = 0;

    protected override void Start()
    {
        base.Start();

        CurrentIndex = _minIndex;
        _maxIndex = Slots.Length - 1;

        Slots[CurrentIndex].ToggleHighlight();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        PlayerInputManager.Hotbar1Event += SetIndex;
        PlayerInputManager.Hotbar2Event += SetIndex;
        PlayerInputManager.Hotbar3Event += SetIndex;
        PlayerInputManager.Hotbar4Event += SetIndex;
        PlayerInputManager.Hotbar5Event += SetIndex;
        PlayerInputManager.AttackEvent += UseItem;

        if (_inventorySystem == null)
        {
            _inventorySystem = InventoryHolder.PrimaryInventorySystem;
        }

        OnHotbarItemAdded += UpdateEquipment;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        PlayerInputManager.Hotbar1Event -= SetIndex;
        PlayerInputManager.Hotbar2Event -= SetIndex;
        PlayerInputManager.Hotbar3Event -= SetIndex;
        PlayerInputManager.Hotbar4Event -= SetIndex;
        PlayerInputManager.Hotbar5Event -= SetIndex;
        OnHotbarItemAdded -= UpdateEquipment;
    }

    private void SetIndex(int index)
    {
        Slots[CurrentIndex].ToggleHighlight();

        if (index < 0)
            index = 0;

        if (index >= _maxIndex)
            index = _maxIndex;

        Debug.Log($"SetIndex {this.gameObject.name}");
        CurrentIndex = index;
        Slots[CurrentIndex].ToggleHighlight();
        OnItemEquip?.Invoke(Slots[CurrentIndex].AssignedInventorySlot.ItemData);
    }

    private void UseItem()
    {
        if (Slots[CurrentIndex].AssignedInventorySlot.ItemData != null)
        {
            Slots[CurrentIndex].AssignedInventorySlot.ItemData.UseItem();
        }
    }

    private void UpdateEquipment(InventorySlot slot)
    {
        Debug.Log(slot.SlotID + " " + CurrentIndex);
        if (slot.SlotID == CurrentIndex)
        {
            OnCurrentSlotItemAdded.Invoke(slot);
        }
    }
}
