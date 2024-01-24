using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInventoryHolder), typeof(PlayerAnimationManager))]
public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private Transform _itemSocket;

    public static event UnityAction OnMainWeaponEquipped;
    public static event UnityAction OnSecondaryWeaponEquipped;
    public static event UnityAction OnItemEquip;
    public static event UnityAction UseCurrentItem;
    public static event UnityAction<PlayerInventoryHolder, PlayerAnimationManager> ReloadWeapon;

    public GameObject _currentEquippedItem;
    private InventoryItemData _currentInventoryItemData;
    public IRangeWeapon _currentRangeWeapon;

    private PlayerInventoryHolder _inventoryHolder;
    private PlayerAnimationManager _animationManager;

    private void Awake()
    {
        _inventoryHolder = GetComponent<PlayerInventoryHolder>();
        _animationManager = GetComponent<PlayerAnimationManager>();
    }

    private void OnEnable()
    {
        HotbarDisplay.OnItemEquip += EquipSlotItem;
        HotbarDisplay.OnCurrentSlotItemChanged += UpdateEquipment;
        PlayerInputManager.AttackEvent += UseItem;
        PlayerInputManager.ReloadEvent += Reload;
    }

    private void OnDisable()
    {
        HotbarDisplay.OnItemEquip -= EquipSlotItem;
        HotbarDisplay.OnCurrentSlotItemChanged -= UpdateEquipment;
        PlayerInputManager.AttackEvent -= UseItem;
    }

    private void UseItem()
    {
        if (_currentInventoryItemData != null)
        {
            _currentInventoryItemData.UseItem(this);
        }
    }

    private void Reload()
    {
        ReloadWeapon?.Invoke(_inventoryHolder, _animationManager);
    }

    private void EquipSlotItem(InventoryItemData itemData)
    {
        ClearSlotIten();

        if (itemData == null)
        {
            return;
        }

        _currentInventoryItemData = itemData;
        if (itemData.ItemModel != null)
            _currentEquippedItem = Instantiate(itemData.ItemModel, _itemSocket);

        // get the IAttackingItem
        itemData.EquipItem(this);
    }

    private void ClearSlotIten()
    {
        if (_currentEquippedItem != null)
        {
            _currentInventoryItemData.UnequipItem(this);
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
