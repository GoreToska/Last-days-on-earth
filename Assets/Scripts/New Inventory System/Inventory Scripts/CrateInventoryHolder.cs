using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrateInventoryHolder : InventoryHolder, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    private List<MeshRenderer> _spriteRenderer = new List<MeshRenderer>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var child in transform.GetComponentsInChildren<MeshRenderer>())
        {
            _spriteRenderer.Add(child.GetComponent<MeshRenderer>());
        }
    }

    public void Interact(Interactor interactor, out bool result, IInputController inputController = null)
    {
        OnDinamicInventoryDisplayRequested?.Invoke(PrimaryInventorySystem);
        result = true;
        SetOutline(1.03f);

        if (inputController != null)
        {
            inputController.DisablePlayerControls();
            inputController.EnableInventoryControls();
        }
    }

    public void EndInteraction()
    {
        SetOutline(1f);
        Debug.Log("End Interact");
    }

    private void SetOutline(float scale)
    {
        foreach (var renderer in _spriteRenderer)
        {
            renderer.materials[1].SetFloat("_Scale", scale);
        }
    }
}
