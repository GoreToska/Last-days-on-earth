using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour, IPickUp
{
    [field: SerializeField] public InventoryItemData ItemData { get; protected set; }
    private SphereCollider collider;

    private void Awake()
    {
        collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();

        if (!inventory)
            return;

        if (inventory.AddToInventory(ItemData, 1))
        {
            Destroy(gameObject);
        }
    }

    public void PickUp()
    {
        throw new System.NotImplementedException();
    }
}

public interface IPickUp
{
    public void PickUp();
}