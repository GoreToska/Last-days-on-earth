using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

[RequireComponent(typeof(PlayerInventoryHolder), typeof(PlayerAnimationManager))]
public class PlayerEquipment : MonoBehaviour
{
	[HideInInspector] public static PlayerEquipment Instance;

	[SerializeField] private Transform _itemSocket;

	[Inject] private DiContainer _container;

	public static event UnityAction OnMainWeaponEquipped;
	public static event UnityAction OnSecondaryWeaponEquipped;
	public static event UnityAction OnItemEquip;
	public static event UnityAction UseCurrentItem;
	public static event UnityAction<PlayerInventoryHolder, PlayerAnimationManager> ReloadWeapon;

	public GameObject _currentEquippedItem;
	private InventoryItemData _currentInventoryItemData;
	public IRangeWeapon _currentRangeWeapon;

	private PlayerStatusManager _statusManager;
	private PlayerInventoryHolder _inventoryHolder;
	private PlayerAnimationManager _animationManager;

	public PlayerStatusManager StatusManager => _statusManager;
	public PlayerAnimationManager AnimationManager => _animationManager;
	public PlayerInventoryHolder InventoryHolder => _inventoryHolder;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		_statusManager = GetComponent<PlayerStatusManager>();
		_inventoryHolder = GetComponent<PlayerInventoryHolder>();
		_animationManager = GetComponent<PlayerAnimationManager>();
	}

	private void OnEnable()
	{
		HotbarDisplay.OnItemEquip += EquipSlotItem;
		HotbarDisplay.OnCurrentSlotItemChanged += UpdateEquipment;
		PlayerInputManager.AttackEvent += UseItem;
		PlayerInputManager.ReloadEvent += Reload;
		HotbarDisplay.OnSlotCleared += ClearEquipmentSlot;
	}

	private void OnDisable()
	{
		HotbarDisplay.OnItemEquip -= EquipSlotItem;
		HotbarDisplay.OnCurrentSlotItemChanged -= UpdateEquipment;
		PlayerInputManager.AttackEvent -= UseItem;
		PlayerInputManager.ReloadEvent -= Reload;
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
		ClearSlotItem();

		if (itemData == null)
		{
			_currentInventoryItemData = null;
			_animationManager.SetWeaponAnimationPattern(WeaponType.None);

			return;
		}

		_currentInventoryItemData = itemData;

		if (itemData.ItemModel != null)
		{
			_currentEquippedItem = _container.InstantiatePrefab(itemData.ItemModel, _itemSocket);
		}

		// get the IAttackingItem
		itemData.EquipItem(this);
	}

	private void ClearSlotItem()
	{
		if (_currentEquippedItem != null)
		{
			_currentInventoryItemData.UnequipItem(this);
			_currentRangeWeapon = null;
			Destroy(_currentEquippedItem);
			_currentInventoryItemData = null;
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
			ClearSlotItem();
		}
	}

	public InventoryRifleData GetCurrentWeapon()
	{
		if (_currentRangeWeapon == null || _currentInventoryItemData == null)
			return null;

		if (_currentInventoryItemData is InventoryRifleData)
			return _currentInventoryItemData as InventoryRifleData;

		return null;
	}

	public void ClearEquipmentSlot()
	{
		ClearSlotItem();
	}

	public void PlayMagazineOutSFX()
	{
		_currentEquippedItem.GetComponent<WeaponActionSFX>().PlayMagazineOutSFX();
	}

	public void PlayMagazineInSFX()
	{
		_currentEquippedItem.GetComponent<WeaponActionSFX>().PlayMagazineInSFX();
	}

	public void PlayChamberSFX()
	{
		_currentEquippedItem.GetComponent<WeaponActionSFX>().PlayChamberSFX();
	}
}
