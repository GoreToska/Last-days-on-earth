using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public PlayerInventoryHolder Inventory { get; private set; }
    public LayerMask InteractionLayer;
   
    private List<IInteractable> _interactables = new List<IInteractable>();
    int CurrentInt = 0;

    public bool IsInteracting { get; private set; }

    private void Awake()
    {
        Inventory = GetComponent<PlayerInventoryHolder>();
    }

    private void OnEnable()
    {
        PlayerInputManager.PickUpEvent += StartInteraction;
        PlayerInputManager.ChangeInteractable += ChangeInteractableNumber;
    }

    private void OnDisable()
    {
        PlayerInputManager.PickUpEvent -= StartInteraction;
        PlayerInputManager.ChangeInteractable -= ChangeInteractableNumber;
    }

    public void ChangeInteractableNumber(float value)
    {
        if (_interactables.Count == 0)
            return;

        ClampCurrentInteractableNumber();
        _interactables[CurrentInt].HighlightInteracable(true);

        value = Mathf.Clamp(value, -1, 1);
        CurrentInt += (int)value;

        if (CurrentInt < 0)
        {
            CurrentInt = _interactables.Count - 1;
        }

        if (CurrentInt > _interactables.Count - 1)
        {
            CurrentInt = 0;
        }

        _interactables[CurrentInt].HighlightCurrentInteractable(true);
    }

    public int AddToInteractionList(IInteractable interactable)
    {
        _interactables.Add(interactable);
        Debug.Log(_interactables.Count);
        return _interactables.Count;
    }

    public void RemoveFromInteractionList(IInteractable interactable)
    {
        _interactables.Remove(interactable);
        SetNewCurrentHighlight();
    }

    private void SetNewCurrentHighlight()
    {
        ClampCurrentInteractableNumber();

        if (_interactables.Count == 0)
            return;

        _interactables[CurrentInt].HighlightCurrentInteractable(true);
    }

    private void StartInteraction()
    {
        if (_interactables.Count == 0)
        {
            return;
        }

        ClampCurrentInteractableNumber();
        _interactables[CurrentInt].Interact(this, out bool result, PlayerInputManager.Instance);
        IsInteracting = result;

        if (_interactables.Count == 0)
            return;

        SetNewCurrentHighlight();
    }

    private void ClampCurrentInteractableNumber()
    {
        if (CurrentInt < 0)
        {
            CurrentInt = 0;
        }

        if (CurrentInt > _interactables.Count - 1)
        {
            CurrentInt = _interactables.Count - 1;
        }
    }

    private void EndInteraction()
    {
        _interactables.RemoveAt(0);
        IsInteracting = false;
    }
}
