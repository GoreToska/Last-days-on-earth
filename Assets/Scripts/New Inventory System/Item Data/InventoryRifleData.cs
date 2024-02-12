using UnityEngine;

public interface IRangeWeapon
{
	public void PerformShot();

	public void PerformReload(PlayerInventoryHolder playerInventory, PlayerAnimationManager playerAnimationManager);
}

[CreateAssetMenu(menuName = "Inventory System/Item/Inventory Weapon Item", fileName = "New Item")]
public class InventoryRifleData : InventoryItemData
{
	[SerializeField] private WeaponData _weaponData;

	public WeaponData WeaponData => _weaponData;

	public override void UseItem(PlayerEquipment playerEquipment)
	{
		base.UseItem(playerEquipment);

		playerEquipment._currentRangeWeapon.PerformShot();
	}

	public override void EquipItem(PlayerEquipment playerEquipment)
	{
		base.EquipItem(playerEquipment);

		playerEquipment.AnimationManager.SetWeaponAnimationPattern(_weaponData.WeaponType);
		playerEquipment._currentRangeWeapon = playerEquipment._currentEquippedItem.GetComponent<IRangeWeapon>();
		PlayerEquipment.ReloadWeapon += playerEquipment._currentRangeWeapon.PerformReload;

	}

	public override void UnequipItem(PlayerEquipment playerEquipment)
	{
		base.UnequipItem(playerEquipment);

		PlayerEquipment.ReloadWeapon -= playerEquipment._currentRangeWeapon.PerformReload;
	}
}
