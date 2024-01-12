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
    [Header("Throwing items params")]
    [SerializeField] private float _heightOffset;
    [SerializeField] private float _rangeOffset;
    [SerializeField] private float _minThrowPower;
    [SerializeField] private float _maxThrowPower;

    public Image ItemSprite;
    public TMP_Text ItemCount;
    public InventorySlot AssignedInventorySlot;

    private Transform _playerTransform;

    private void Awake()
    {
        ItemSprite.preserveAspect = true;
        ItemSprite.color = Color.clear;
        ItemCount.text = "";

        //  TODO: get it from somewere else (zenject mb)
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (_playerTransform == null)
        {
            Debug.LogWarning("Player cant be found.");
        }
    }

    private void Update()
    {
        if (AssignedInventorySlot.ItemData != null)
        {
            transform.position = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                if (AssignedInventorySlot.ItemData.ItemPrefab != null)
                {
                    var item = Instantiate(AssignedInventorySlot.ItemData.ItemPrefab,
                         _playerTransform.position + _playerTransform.up * _heightOffset + _playerTransform.forward * _rangeOffset,
                         Quaternion.identity);

                    item.GetComponent<Rigidbody>().AddForce(_playerTransform.forward * Random.Range(_minThrowPower, _maxThrowPower), ForceMode.Impulse);
                }

                if (AssignedInventorySlot.StackSize > 1)
                {
                    AssignedInventorySlot.AddToStack(-1);
                    UpdateMouseSlot();
                }
                else
                {
                    ClearSlot();
                }
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
        UpdateMouseSlot();
    }

    public void UpdateMouseSlot()
    {
        ItemSprite.sprite = AssignedInventorySlot.ItemData.Icon;
        if (AssignedInventorySlot.StackSize > 1)
            ItemCount.text = AssignedInventorySlot.StackSize.ToString();
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
