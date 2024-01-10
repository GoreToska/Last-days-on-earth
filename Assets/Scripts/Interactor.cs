using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public Transform InteractionPoint;
    public LayerMask InteractionLayer;
    public float InteractionPointRadius = 1f;
    public bool IsInteracting { get; private set; }

    private void InteractWithItems()
    {
        var colliders = Physics.OverlapSphere(InteractionPoint.position, InteractionPointRadius, InteractionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            var interactable = colliders[i].GetComponent<IInteractable>();

            if (interactable != null)
            {
                StartInteraction(interactable);
            }
        }
    }

    private void OnEnable()
    {
        PlayerInputManager.Instance.PickUpEvent += InteractWithItems;
    }

    private void OnDisable()
    {
        PlayerInputManager.Instance.PickUpEvent -= InteractWithItems;
    }

    private void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool result, PlayerInputManager.Instance);
        IsInteracting = result;
    }

    private void EndInteraction()
    {
        IsInteracting = false;
    }
}
