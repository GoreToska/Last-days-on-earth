using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;

public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private MouseItemData _mouseInventoryItem;

    [Inject] private HotbarDisplay _hotbarDisplay;

    protected InventorySystem _inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> _slotDictionary;

    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => _slotDictionary;
    public InventorySystem InventorySystem => _inventorySystem;

    protected virtual void Start()
    {

    }

    public abstract void AssignSlot(InventorySystem inventoryToDisplay, int offset);

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot) // under the hood inventory slot
            {
                slot.Key.UpdateUISlot(updatedSlot); // ui represetnation of value
            }
        }
    }

    public void SlotClicked(InventorySlot_UI clickedUISlot)
    {
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;

        if (clickedUISlot.AssignedInventorySlot.ItemData != null && _mouseInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            // if player is holding shift - split stack 
            if (isShiftPressed && clickedUISlot.AssignedInventorySlot.SplitStack(out var halfStackSlot)) // split stack
            {
                _mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedUISlot.UpdateUISlot();
                return;
            }
            else
            {
                // clicked slot has an item and mouse doesn't have an item - pick up item
                _mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
                _hotbarDisplay.ClearEquipmentSlot(clickedUISlot.AssignedInventorySlot);
				clickedUISlot.ClearSlot();

                return;
            }
        }

        // clicked slot doesn't have an item - mouse have an item - place item 
        if (clickedUISlot.AssignedInventorySlot.ItemData == null && _mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            clickedUISlot.AssignedInventorySlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
            clickedUISlot.UpdateUISlot();
            _hotbarDisplay.UpdateEquipment(clickedUISlot.AssignedInventorySlot);

			_mouseInventoryItem.ClearSlot();
        }

        // both have an item - decide what to do
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && _mouseInventoryItem.AssignedInventorySlot.ItemData != null)
        {
            bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == _mouseInventoryItem.AssignedInventorySlot.ItemData;

            //if different items - swap them
            
            // are both items are the same? - combine
            if (isSameItem &&
                clickedUISlot.AssignedInventorySlot.SpaceLeftInStack(_mouseInventoryItem.AssignedInventorySlot.StackSize))
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
                clickedUISlot.UpdateUISlot();

                _mouseInventoryItem.ClearSlot();
                
                return;
            }
            else if (isSameItem &&
                !clickedUISlot.AssignedInventorySlot.SpaceLeftInStack(_mouseInventoryItem.AssignedInventorySlot.StackSize, out var amountRemaining))
            {
                if (amountRemaining < 1) // stack is full so swap
                {
                    SwapSlots(clickedUISlot);
                    return;
                }
                else // slot is not at max so take what is needed
                {
                    // if the slot stack size + mouse stack size > the slot max stack size? - take from mouse

                    int remainingOnMouse = _mouseInventoryItem.AssignedInventorySlot.StackSize - amountRemaining;
                    
                    clickedUISlot.AssignedInventorySlot.AddToStack(amountRemaining);
                    clickedUISlot.UpdateUISlot();

                    var newItem = new InventorySlot(_mouseInventoryItem.AssignedInventorySlot.ItemData, remainingOnMouse);
                    _mouseInventoryItem.ClearSlot();
                    _mouseInventoryItem.UpdateMouseSlot(newItem);
                    
                    return;
                }
            }
            else if(!isSameItem)
            {
                SwapSlots(clickedUISlot);
                return;
            }
        }

    }

    private void SwapSlots(InventorySlot_UI clickedUISlot)
    {
        var clonedSlot = new InventorySlot(_mouseInventoryItem.AssignedInventorySlot.ItemData, _mouseInventoryItem.AssignedInventorySlot.StackSize);
        _mouseInventoryItem.ClearSlot();

        _mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);

        clickedUISlot.ClearSlot();
        clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();
    }
}
