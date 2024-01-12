using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public PlayerInventoryHolder Inventory { get; private set; }
    public Transform InteractionPoint;
    public LayerMask InteractionLayer;
    public float InteractionPointRadius = 1f;
    private List<IInteractable> _interactables = new List<IInteractable>();
    int CurrentInt = 0;
    //public List<IInteractable> Interactables
    //{
    //    get { _interactables.RemoveAll(i => i == null); return _interactables; }
    //    set { _interactables.RemoveAll(i => i == null); _interactables = value; }
    //}

    public bool IsInteracting { get; private set; }

    //private void InteractWithItems()
    //{
    //    var colliders = Physics.OverlapSphere(InteractionPoint.position, InteractionPointRadius, InteractionLayer);

    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        var interactable = colliders[i].GetComponent<IInteractable>();

    //        if (interactable != null)
    //        {
    //            StartInteraction(interactable);
    //        }
    //    }
    //}

    private void Awake()
    {
        Inventory = GetComponent<PlayerInventoryHolder>();
    }

    private void OnEnable()
    {
        PlayerInputManager.PickUpEvent += StartInteraction;
    }

    private void OnDisable()
    {
        PlayerInputManager.PickUpEvent -= StartInteraction;
    }

    public void AddToInteractionList(IInteractable interactable)
    {
        _interactables.Add(interactable);
    }

    public void RemoveFromInteractionList(IInteractable interactable)
    {
        _interactables.Remove(interactable);
    }

    private void StartInteraction()
    {
        if (_interactables.Count == 0)
        {
            return;
        }

        _interactables[CurrentInt].Interact(this, out bool result, PlayerInputManager.Instance);
        IsInteracting = result;
    }

    private void EndInteraction()
    {
        _interactables.RemoveAt(0);
        IsInteracting = false;
    }
}
