using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    [HideInInspector] public static PlayerUIManager Instance;

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
    }

    private void OnEnable()
    {
        //PlayerInputManager.Instance.OpenInventoryEvent
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
        staminaBar.SetStatus(value);
    }

}
