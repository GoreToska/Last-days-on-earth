using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Item : MonoBehaviour
{
    //[Header("If this item can be equipped in hands")]
    //[SerializeField] public GameObject itemPrefab;

    //[Header("If this item is weapon")]
    //[SerializeField] public WeaponData weaponData;

    private void OnTriggerEnter(Collider other)
    {
        PlayerEquipmentManager.Instance.itemsToPickUp.Add(this);
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerEquipmentManager.Instance.itemsToPickUp.Remove(this);
    }

    public virtual bool PickUpItem()
    {
        return false;
    }
}
