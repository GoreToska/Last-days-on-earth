using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private Transform _itemSocket;

    private GameObject _currentEquippedItem;

    private void OnEnable()
    {
        HotbarDisplay.OnItemEquip += EquipSlotItem;
        HotbarDisplay.OnCurrentSlotItemAdded += UpdateEquipment;
    }

    private void OnDisable()
    {
        HotbarDisplay.OnItemEquip -= EquipSlotItem;
        HotbarDisplay.OnCurrentSlotItemAdded -= UpdateEquipment;
    }

    private void EquipSlotItem(InventoryItemData itemData)
    {
        if (_currentEquippedItem != null)
        {
            Destroy(_currentEquippedItem);
        }

        if (itemData == null)
        {
            return;
        }

        _currentEquippedItem = Instantiate(itemData.ItemModel, _itemSocket);
    }

    private void UpdateEquipment(InventorySlot slot)
    {
        EquipSlotItem(slot.ItemData);
    }
}
