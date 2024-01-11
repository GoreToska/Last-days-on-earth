using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour, IPickUp
{
    [field: SerializeField] public InventoryItemData ItemData { get; protected set; }
    private SphereCollider collider;
    private string id;
    [SerializeField] private ItemPickUpSaveData itemSaveData;

    private void Awake()
    {
        id = GetComponent<UniqueID>().ID;
        SaveLoad.OnLoadGame += LoadGame;
        itemSaveData = new ItemPickUpSaveData(ItemData, transform.position, transform.rotation);

        collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
    }

    private void Start()
    {
        SaveGameManager.data.ActiveItems.Add(id, itemSaveData);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag != "Player")
            return;

        var inventory = other.transform.root.GetComponent<PlayerInventoryHolder>();

        if (!inventory)
            return;

        if (inventory.AddToInventory(ItemData, 1))
        {
            SaveGameManager.data.CollectedItems.Add(id);
            Destroy(gameObject);
        }
    }

    private void LoadGame(SaveData data)
    {
        if (data.CollectedItems.Contains(id))
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (SaveGameManager.data.ActiveItems.ContainsKey(id))
            SaveGameManager.data.ActiveItems.Remove(id);

        SaveLoad.OnLoadGame -= LoadGame;
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

[System.Serializable]
public struct ItemPickUpSaveData
{
    public InventoryItemData ItemData;
    public Vector3 Position;
    public Quaternion Rotation;

    public ItemPickUpSaveData(InventoryItemData data, Vector3 position, Quaternion rotation)
    {
        ItemData = data;
        Position = position;
        Rotation = rotation;
    }
}