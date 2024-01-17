using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private Transform _itemSocket;

    public static event UnityAction OnMainWeaponEquipped;
    public static event UnityAction OnSecondaryWeaponEquipped;
    public static event UnityAction OnItemEquip;
    public static event UnityAction UseCurrentItem;

    public GameObject _currentEquippedItem;
    private InventoryItemData _currentInventoryItemData;
    public IRangeWeapon _currentRangeWeapon;

    private void OnEnable()
    {
        HotbarDisplay.OnItemEquip += EquipSlotItem;
        HotbarDisplay.OnCurrentSlotItemChanged += UpdateEquipment;
        PlayerInputManager.AttackEvent += UseItem;
    }

    private void OnDisable()
    {
        HotbarDisplay.OnItemEquip -= EquipSlotItem;
        HotbarDisplay.OnCurrentSlotItemChanged -= UpdateEquipment;
        PlayerInputManager.AttackEvent -= UseItem;
    }

    private void UseItem()
    {
        if(_currentInventoryItemData != null )
        {
            _currentInventoryItemData.UseItem(this);
        }
    }

    private void EquipSlotItem(InventoryItemData itemData)
    {
        ClearSlotIten();

        if (itemData == null)
        {
            return;
        }

        _currentInventoryItemData = itemData;
        _currentEquippedItem = Instantiate(itemData.ItemModel, _itemSocket);
        // get the IAttackingItem
        itemData.EquipItem(this);
    }

    private void ClearSlotIten()
    {
        if (_currentEquippedItem != null)
        {
            Destroy(_currentEquippedItem);
        }
    }

    private void UpdateEquipment(InventorySlot slot)
    {
        if (slot.ItemData != null)
        {
            EquipSlotItem(slot.ItemData);

        }
        else
        {
            ClearSlotIten();
        }
    }
}
