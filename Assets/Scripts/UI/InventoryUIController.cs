using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private DinamicInventoryDisplay _playerBackpackPanel;
    [SerializeField] private DinamicInventoryDisplay _inventoryPanel;
    [SerializeField] private PlayerInventoryHolder _playerInventory;

    [Inject] private InventoryItemDescriptionManager _descriptionManager;

	private void Start()
    {
        CloseChest();
        CloseInventory();
    }

    private void OnEnable()
    {
        PlayerInputManager.CloseInventoryEvent += () => { CloseInventory(); CloseChest(); };
        PlayerInputManager.AlternativeCloseInventoryEvent += () => { CloseInventory(); CloseChest(); };
        InventoryHolder.OnDinamicInventoryDisplayRequested += DisplayInventory;
    }

    private void OnDisable()
    {
        PlayerInputManager.CloseInventoryEvent -= () => { CloseInventory(); CloseChest(); };
        PlayerInputManager.AlternativeCloseInventoryEvent -= () => { CloseInventory(); CloseChest(); };
        InventoryHolder.OnDinamicInventoryDisplayRequested -= DisplayInventory;
    }

    private void DisplayInventory(InventorySystem inventoryToDisplay, int offset)
    {
        DisplayPlayerBackpack();

        if (offset == _playerInventory.Offset)
            return;
        else
        {
            DisplayCrateInventory(inventoryToDisplay, offset);
        }
    }

    private void DisplayPlayerBackpack()
    {
        _playerBackpackPanel.gameObject.SetActive(true);
        _playerBackpackPanel.RefreshDinamycInventory(_playerInventory.PrimaryInventorySystem, _playerInventory.Offset);
    }

    private void DisplayCrateInventory(InventorySystem inventoryToDisplay, int offset)
    {
        _inventoryPanel.gameObject.SetActive(true);
        _inventoryPanel.RefreshDinamycInventory(inventoryToDisplay, offset);
    }

    private void CloseInventory()
    {
        _playerBackpackPanel.gameObject.SetActive(false);
		_descriptionManager.HideDescriptionPanel();
	}

    private void CloseChest()
    {
        _inventoryPanel.gameObject.SetActive(false);
		_descriptionManager.HideDescriptionPanel();
	}
}
