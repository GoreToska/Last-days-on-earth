using UnityEngine;

public class AmmoItem : Item
{
    [SerializeField] private int _count;

    public override void PickUpItem()
    {
        Debug.Log("Pickup");
        Destroy(gameObject);
    }
}
