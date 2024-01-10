using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrateInventory : InventoryHolder, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void Interact(Interactor interactor, out bool result, IInputController inputController = null)
    {
        OnDinamicInventoryDisplayRequested?.Invoke(InventorySystem);
        result = true;

        if (inputController != null)
        {
            inputController.DisablePlayerControls();
            inputController.EnableInventoryControls();
            Debug.Log("Disable");
        }
    }

    public void EndInteraction()
    {
        Debug.Log("End Interact");
    }
}
