using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUIManager : MonoBehaviour
{
    [HideInInspector] public static PlayerUIManager Instance;

    [Header("UI Windows")]
    [SerializeField] private UIDocument inventory;
    [SerializeField] private UIDocument HUD;

    [Header("UI Managers")]
    [SerializeField] private InventoryUIController _inventoryUIController;

    [Header("Status Bars")]
    [SerializeField] private StatusBar _hpBar;
    [SerializeField] private StatusBar _staminaBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        OnCloseInventory();
    }

    private void OnEnable()
    {
        PlayerInputManager.Instance.OpenInventoryEvent += OnOpenInventory;
        PlayerInputManager.Instance.CloseInventoryEvent += OnCloseInventory;
    }

    private void OnDisable()
    {
        PlayerInputManager.Instance.OpenInventoryEvent -= OnOpenInventory;
        PlayerInputManager.Instance.CloseInventoryEvent -= OnCloseInventory;
    }

    public void SetHP(float value)
    {
        _hpBar.SetStatus(value);
    }

    public void SetStamina(float value)
    {
        //staminaBar.SetStatus(value);
    }

    private void OnOpenInventory()
    {
        Debug.Log("Заглушка 2");
        //PlayerInventory.Instance.InventoryVisibility(Visibility.Visible);
    }

    private void OnCloseInventory()
    {
        Debug.Log("Заглушка");
        //PlayerInventory.Instance.InventoryVisibility(Visibility.Hidden);
        //PlayerInventory.Instance.TelegraphVisibility(Visibility.Hidden);
    }
}
