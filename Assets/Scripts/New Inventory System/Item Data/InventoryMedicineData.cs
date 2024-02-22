using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Item/Inventory Med Item", fileName = "New Item")]
public class InventoryMedicineData : InventoryItemData
{
	[field: SerializeField] public float HPRestoration = 50;

	public override void UseItem(PlayerEquipment playerEquipment)
	{
		base.UseItem(playerEquipment);

        playerEquipment.StatusManager.RegenHP(HPRestoration);
		playerEquipment.InventoryHolder.RemoveFromInventory(this, 1);
	}

	public override void UnequipItem(PlayerEquipment playerEquipment)
	{
		base.UnequipItem(playerEquipment);
	}

	public override void EquipItem(PlayerEquipment playerEquipment)
	{
		base.EquipItem(playerEquipment);

		playerEquipment.AnimationManager.SetWeaponAnimationPattern(WeaponType.None);
	}
}
