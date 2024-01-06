using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseItemData : MonoBehaviour
{
    public Image ItemSprite;
    public TMP_Text ItemCount;
    public InventorySlot AssignedInventorySlot;

    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemCount.text = "";
    }

    private void Update()
    {
        // TODO change to new input system?
        if (AssignedInventorySlot.ItemData != null)
        {
            transform.position = Mouse.current.position.ReadValue();

            Debug.Log(IsPointerOverUIObject());
            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                ClearSlot();
            }
        }
    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
    }

    public void UpdateMouseSlot(InventorySlot slot)
    {
        AssignedInventorySlot.AssignItem(slot);
        ItemSprite.sprite = slot.ItemData.Icon;
        if (slot.StackSize > 1)
            ItemCount.text = slot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }

    // TODO move to other class
    public static bool IsPointerOverUIObject()
    {
        PointerEventData currentPositionEventData = new PointerEventData(EventSystem.current);
        currentPositionEventData.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(currentPositionEventData, results);

        return results.Count > 0;
    }
}
