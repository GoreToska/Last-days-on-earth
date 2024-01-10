using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestInventory : InventoryHolder, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void Interact(Interactor interactor, out bool result)
    {
        OnDinamicInventoryDisplayRequested?.Invoke(InventorySystem);
        result = true;
    }

    public void EndInteraction()
    {
        Debug.Log("End Interact");
    }
}
