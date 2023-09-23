using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUIManager : MonoBehaviour
{
    [HideInInspector] public static PlayerUIManager Instance;

    [Header("UI Windows")]
    [SerializeField] private UIDocument inventory;

    [Header("Status Bars")]
    [SerializeField] private StatusBar hpBar;
    [SerializeField] private StatusBar staminaBar;

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

    }

    public void SetHP(float value)
    {
        hpBar.SetStatus(value);
    }

    public void SetStamina(float value)
    {
        //staminaBar.SetStatus(value);
    }

    private void OnOpenInventory()
    {
        Debug.Log("Open");
        PlayerInventory.Instance.InventoryVisibility(Visibility.Visible);
    }

    private void OnCloseInventory()
    {
        PlayerInventory.Instance.InventoryVisibility(Visibility.Hidden);
        PlayerInventory.Instance.TelegraphVisibility(Visibility.Hidden);
    }
}
