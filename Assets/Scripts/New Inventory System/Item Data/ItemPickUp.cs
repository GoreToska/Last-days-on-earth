using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider), typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour, IInteractable
{
    [field: SerializeField] public InventoryItemData ItemData { get; protected set; }
    public UnityAction<IInteractable> OnInteractionComplete { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private SphereCollider collider;
    private string id;
    private ItemPickUpSaveData itemSaveData;

    private void Awake()
    {
        SaveLoad.OnLoadGame += LoadGame;
        itemSaveData = new ItemPickUpSaveData(ItemData, transform.position, transform.rotation);

        collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
    }

    private void Start()
    {
        id = GetComponent<UniqueID>().ID;
        SaveGameManager.data.ActiveItems.Add(id, itemSaveData);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        var interactor = other.transform.root.GetComponent<Interactor>();
        interactor.AddToInteractionList(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        var interactor = other.transform.root.GetComponent<Interactor>();
        interactor.RemoveFromInteractionList(this);
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

    public void Interact(Interactor interactor, out bool result, IInputController inputController = null)
    {
        if (interactor.Inventory.AddToInventory(ItemData, 1))
        {
            SaveGameManager.data.CollectedItems.Add(id);
            result = true;
            EndInteraction(interactor);

            return;
        }

        result = false;
    }

    public void EndInteraction(Interactor interactor)
    {
        interactor.RemoveFromInteractionList(this as IInteractable);
        UnityEngine.Object.Destroy(this.gameObject);
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