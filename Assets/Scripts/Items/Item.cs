using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemDefinition itemDefinition;

    [Header("If this item can be equipped in hands")]
    [SerializeField] public GameObject itemPrefab;

    [Header("If this item is weapon")]
    [SerializeField] public WeaponData weaponData;

    private void OnTriggerEnter(Collider other)
    {
        PlayerEquipmentManager.Instance.itemsToPickUp.Add(this);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerEquipmentManager.Instance.itemsToPickUp.Remove(this);
    }

    public async Task<bool> PickUpItem()
    {
        var item = await PlayerInventory.Instance.AddNewItem(itemDefinition);

        if (item != null)
        {
            Destroy(gameObject);

            // UI popup "YOU TAKE ITEM_NAME"
            if (weaponData != null && weaponData.weaponType == WeaponType.Primary)
            {
                PlayerEquipmentManager.Instance.OnMainWeaponEquip(this, item);
            }

            return true;
        }

        //  UI popup "YOU CANT PICK UP"
        return false;
    }
}
