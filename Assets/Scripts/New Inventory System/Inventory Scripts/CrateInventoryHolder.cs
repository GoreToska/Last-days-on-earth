using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[RequireComponent(typeof(UniqueID), typeof(SphereCollider))]
public class CrateInventoryHolder : InventoryHolder, IInteractable
{
    [SerializeField] private InteractableHighlightConfig _interactableHighlightConfig;
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

        if (interactor.AddToInteractionList(this) == 1)
        {
            HighlightCurrentInteractable(true);
        }
        else
        {
            HighlightInteracable(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        var interactor = other.transform.root.GetComponent<Interactor>();
        interactor.RemoveFromInteractionList(this);
        HighlightInteracable(false);
    }

    public void Interact(Interactor interactor, out bool result, IInputController inputController = null)
    {
        OnDinamicInventoryDisplayRequested?.Invoke(PrimaryInventorySystem, 0); // pass no offset
        result = true;
        //HighlightInteracable(true);

        if (inputController != null)
        {
            inputController.DisablePlayerControls();
            inputController.EnableInventoryControls();
        }
    }

    public void EndInteraction(Interactor interactor)
    {
        HighlightInteracable(false);
        Debug.Log("End Interact");
    }

    public void HighlightInteracable(bool value)
    {
        if (value)
        {
            foreach (var renderer in _meshRenderer)
            {
                renderer.materials[1].SetColor("_Color", _interactableHighlightConfig.Highlight);
                renderer.materials[1].SetFloat("_Scale", 1.03f);
            }
        }
        else
        {
            foreach (var renderer in _meshRenderer)
            {
                renderer.materials[1].SetFloat("_Scale", 1f);
            }
        }
    }
    public void HighlightCurrentInteractable(bool value)
    {
        if (value)
        {
            foreach (var renderer in _meshRenderer)
            {
                renderer.materials[1].SetColor("_Color", _interactableHighlightConfig.ChosedHighlight);
                renderer.materials[1].SetFloat("_Scale", 1.03f);
            }
        }
        else
        {
            foreach (var renderer in _meshRenderer)
            {
                renderer.materials[1].SetFloat("_Scale", 1f);
            }
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

    private MaterialPropertyBlock materialPropertyBlock;
}
