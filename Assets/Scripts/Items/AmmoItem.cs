using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AmmoItem : Item
{
    public AmmoData data;

    public override  bool PickUpItem()
    {
        var item = PlayerInventory.Instance.AddNewAmmoItem(data);

        if (item != null)
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
                // UI popup "YOU TAKE ITEM_NAME"
                return true;
            }
            //if (weaponData != null && weaponData.weaponType == WeaponType.Primary)
            //{
            //    PlayerEquipmentManager.Instance.OnMainWeaponEquip(this, item);
            //}
        }

        //  UI popup "YOU CANT PICK UP"
        return false;
    }
}
