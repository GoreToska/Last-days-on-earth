using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private DinamicInventoryDisplay _playerBackpackPanel;
    [SerializeField] private DinamicInventoryDisplay _chestPanel;
    [SerializeField] private PlayerInventoryHolder _playerInventory;

    private void Awake()
    {
        CloseChest();
        CloseInventory();
    }

    private void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested += DisplayPlayerBackpack;
        PlayerInputManager.CloseInventoryEvent += () => { CloseInventory(); CloseChest(); };
        PlayerInputManager.AlternativeCloseInventoryEvent += () => { CloseInventory(); CloseChest(); };
        InventoryHolder.OnDinamicInventoryDisplayRequested += DisplayCrateAndBackpack;
    }

    private void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested -= DisplayPlayerBackpack;
        PlayerInputManager.CloseInventoryEvent -= () => { CloseInventory(); CloseChest(); };
        PlayerInputManager.AlternativeCloseInventoryEvent -= () => { CloseInventory(); CloseChest(); };
        InventoryHolder.OnDinamicInventoryDisplayRequested -= DisplayCrateAndBackpack;
    }

    private void DisplayInventory(InventorySystem inventoryToDisplay)
    {
        _chestPanel.gameObject.SetActive(true);
        _chestPanel.RefreshDinamycInventory(inventoryToDisplay);
    }

    private void DisplayCrateAndBackpack(InventorySystem inventoryToDisplay)
    {
        DisplayInventory(inventoryToDisplay);
        DisplayPlayerBackpack(_playerInventory.SecondaryInventorySystem);
    }

    private void DisplayPlayerBackpack(InventorySystem inventoryToDisplay)
    {
        _playerBackpackPanel.gameObject.SetActive(true);
        _playerBackpackPanel.RefreshDinamycInventory(inventoryToDisplay);
    }

    private void CloseInventory()
    {
        _playerBackpackPanel.gameObject.SetActive(false);
    }

    private void CloseChest()
    {
        _chestPanel.gameObject.SetActive(false);
    }
}
