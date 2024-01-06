using InventorySystem;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WeaponItem : Item
{
    public override void PickUpItem()
    {
        InventoryController.instance.AddItem("Main", data.GetItemType());
        Destroy(gameObject);
    }
}
