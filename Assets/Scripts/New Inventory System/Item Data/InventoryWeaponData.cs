using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangeWeapon
{
    public void PerformShot();

    public void PerformReload();
}

[CreateAssetMenu(menuName = "Inventory System/Item/Inventory Weapon Item", fileName = "New Item")]
public class InventoryWeaponData : InventoryItemData
{
    [SerializeField] private WeaponData weaponData;

    public override void UseItem(PlayerEquipment playerEquipment)
    {
        base.UseItem(playerEquipment);
        playerEquipment._currentRangeWeapon.PerformShot();
    }

    public override void EquipItem(PlayerEquipment playerEquipment)
    {
        base.EquipItem(playerEquipment);
        playerEquipment._currentRangeWeapon = playerEquipment._currentEquippedItem.GetComponent<IRangeWeapon>();
    }
}
