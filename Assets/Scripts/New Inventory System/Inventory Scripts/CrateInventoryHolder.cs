using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID), typeof(SphereCollider))]
public class CrateInventoryHolder : InventoryHolder, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    private List<MeshRenderer> _meshRenderer = new List<MeshRenderer>();
    private SphereCollider _sphereCollider;

    protected override void Awake()
    {
        base.Awake();

        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;

        foreach (var child in transform.GetComponentsInChildren<MeshRenderer>())
        {
            _meshRenderer.Add(child.GetComponent<MeshRenderer>());
        }
    }

    private void Start()
    {
        var crateSaveData = new InventorySaveData(PrimaryInventorySystem, transform.position, transform.rotation);

        SaveGameManager.data.CrateDictionary.Add(id, crateSaveData);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        var interactor = other.transform.root.GetComponent<Interactor>();
        interactor.AddToInteractionList(this);
        SetOutline(1.03f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        var interactor = other.transform.root.GetComponent<Interactor>();
        interactor.RemoveFromInteractionList(this);
        SetOutline(1f);
    }

    public void Interact(Interactor interactor, out bool result, IInputController inputController = null)
    {
        OnDinamicInventoryDisplayRequested?.Invoke(PrimaryInventorySystem, 0); // pass no offset
        result = true;
        SetOutline(1.03f);

        if (inputController != null)
        {
            inputController.DisablePlayerControls();
            inputController.EnableInventoryControls();
        }
    }

    public void EndInteraction(Interactor interactor)
    {
        SetOutline(1f);
        Debug.Log("End Interact");
    }

    private void SetOutline(float scale)
    {
        foreach (var renderer in _meshRenderer)
        {
            renderer.materials[1].SetFloat("_Scale", scale);
        }
    }

    protected override void LoadInventory(SaveData data)
    {
        //check the save data for this specific crate inventory, and if it exists, load it 
        if (data.CrateDictionary.TryGetValue(id, out var crateData))
        {
            this.PrimaryInventorySystem = crateData.InventorySystem;
            this.transform.position = crateData.Position;
            this.transform.rotation = crateData.Rotation;
        }
    }
}
