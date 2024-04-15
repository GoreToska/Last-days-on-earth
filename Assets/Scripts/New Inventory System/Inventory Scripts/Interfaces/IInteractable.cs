using Language.Lua;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    public void Interact(Interactor interactor, out bool result, IInputController inputController = null);
    public void EndInteraction(Interactor interactor);

    public void HighlightInteracable(bool value);

    public void HighlightCurrentInteractable(bool value);

    public Transform GetTransform();

    public string GetName();
}