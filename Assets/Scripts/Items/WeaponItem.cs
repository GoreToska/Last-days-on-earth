using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponItem : Item
{
    [SerializeField] public WeaponData data;

    public override async Task<bool> PickUpItem()
    {
        var success = PlayerInventory.Instance.AddNewWeaponItem(this);

        if (success != null)
        {
            if(gameObject)
            {
                Destroy(gameObject);
            }

            // UI popup "YOU TAKE ITEM_NAME"
            return true;
        }

        //  UI popup "YOU CANT PICK UP"
        return false;
    }
}