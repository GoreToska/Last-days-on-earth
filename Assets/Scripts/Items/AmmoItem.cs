using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AmmoItem : Item
{
    public AmmoData data;

    public override async Task<bool> PickUpItem()
    {
        var item = await PlayerInventory.Instance.AddNewAmmoItem(data);

        if (item != null)
        {
            Destroy(gameObject);

            // UI popup "YOU TAKE ITEM_NAME"

            //if (weaponData != null && weaponData.weaponType == WeaponType.Primary)
            //{
            //    PlayerEquipmentManager.Instance.OnMainWeaponEquip(this, item);
            //}

            return true;
        }

        //  UI popup "YOU CANT PICK UP"
        return false;
    }
}
