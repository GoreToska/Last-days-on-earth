using InventorySystem;
using UnityEngine;

public class AmmoItem : Item
{
    [SerializeField] private int _count;

    public override void PickUpItem()
    {
        InventoryController.instance.AddItem("Main", data.GetItemType(), _count);
        Destroy(gameObject);
    }
}
