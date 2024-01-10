using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    public DinamicInventoryDisplay InventoryPanel;

    private void Awake()
    {
        CloseInventory();
    }

    private void OnEnable()
    {
        PlayerInputManager.Instance.OpenInventoryEvent += OnOpenInventory;
        PlayerInputManager.Instance.CloseInventoryEvent += OnCloseInventory;
        PlayerInputManager.Instance.AlternativeCloseInventoryEvent+= OnCloseInventory;
        InventoryHolder.OnDinamicInventoryDisplayRequested += DisplayInventory;
    }

    private void OnDisable()
    {
        PlayerInputManager.Instance.OpenInventoryEvent -= OnOpenInventory;
        PlayerInputManager.Instance.CloseInventoryEvent += OnCloseInventory;
        PlayerInputManager.Instance.AlternativeCloseInventoryEvent -= OnCloseInventory;
        InventoryHolder.OnDinamicInventoryDisplayRequested -= DisplayInventory;
    }

    private void OnOpenInventory()
    {
        DisplayInventory(new InventorySystem(20));
    }

    private void OnCloseInventory()
    {
        CloseInventory();
    }

    private void DisplayInventory(InventorySystem inventoryToDisplay)
    {
        InventoryPanel.gameObject.SetActive(true);
        InventoryPanel.RefreshDinamycInventory(inventoryToDisplay);
    }

    private void CloseInventory()
    {
        InventoryPanel.gameObject.SetActive(false);
    }
}
