using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;

public class Interactor : MonoBehaviour
{
	[Inject] private InteractablesPromptManager _promptManager;

	public PlayerInventoryHolder Inventory { get; private set; }
	public LayerMask InteractionLayer;

	private List<IInteractable> _interactables = new List<IInteractable>();
	private int _currentInteractableNumber = 0;
	private IInteractable _currentInteractable;

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

		if (_currentInteractable != null)
			_currentInteractable.HighlightInteracable(true);

		ClampCurrentInteractableNumber();


		value = Mathf.Clamp(value, -1, 1);
		_currentInteractableNumber += (int)value;

		if (_currentInteractableNumber < 0)
		{
			_currentInteractableNumber = _interactables.Count - 1;
		}

		if (_currentInteractableNumber > _interactables.Count - 1)
		{
			_currentInteractableNumber = 0;
		}

		_currentInteractable = _interactables[_currentInteractableNumber];
		_promptManager.ShowPrompt(_currentInteractable.GetTransform(), _currentInteractable.GetName());
		_currentInteractable.HighlightCurrentInteractable(true);
	}

	public int AddToInteractionList(IInteractable interactable)
	{
		_interactables.Add(interactable);

		if (_interactables.Count == 1)
		{
			interactable.HighlightCurrentInteractable(true);
			_promptManager.ShowPrompt(interactable.GetTransform(), interactable.GetName());
		}
		else
		{
			interactable.HighlightInteracable(true);
		}

		return _interactables.Count;
	}

	public void RemoveFromInteractionList(IInteractable interactable)
	{
		_interactables.Remove(interactable);
		interactable.HighlightInteracable(false);
		_promptManager.HidePrompt();

		SetNewCurrentHighlight();
	}

	private void SetNewCurrentHighlight()
	{
		ClampCurrentInteractableNumber();

		if (_interactables.Count == 0)
			return;

		if (_currentInteractable != null)
		{
			_currentInteractable.HighlightCurrentInteractable(true);
			//_interactables[_currentInteractableNumber].HighlightCurrentInteractable(true);
			_promptManager.ShowPrompt(_currentInteractable.GetTransform(), _currentInteractable.GetName());
		}
	}

	private void StartInteraction()
	{
		if (_interactables.Count == 0)
		{
			return;
		}

		ClampCurrentInteractableNumber();
		_currentInteractable.Interact(this, out bool result, PlayerInputManager.Instance);
		IsInteracting = result;

		if (_interactables.Count == 0)
			return;

		SetNewCurrentHighlight();
	}

	private void ClampCurrentInteractableNumber()
	{
		if (_currentInteractableNumber < 0)
		{
			_currentInteractableNumber = 0;
		}

		if (_currentInteractableNumber >= _interactables.Count)
		{
			_currentInteractableNumber = _interactables.Count - 1;
		}

		_currentInteractable = _interactables.Count > 0 ? _currentInteractable = _interactables[_currentInteractableNumber] : null;
	}
}
