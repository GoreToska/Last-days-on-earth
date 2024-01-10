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
        InventoryHolder.OnDinamicInventoryDisplayRequested += DisplayInventory;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDinamicInventoryDisplayRequested -= DisplayInventory;
    }

    void Update() // TODO change inputs
    {
        if (!InventoryPanel.gameObject.activeSelf && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            DisplayInventory(new InventorySystem(20));
            Debug.Log("B");
            return;
        }

        if (InventoryPanel.gameObject.activeSelf && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseInventory();
            Debug.Log("B2");
            return;
        }
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
