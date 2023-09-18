using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    [HideInInspector] public static PlayerEquipmentManager Instance;
    
    //  Handle weapon change
    [SerializeField] private MainWeapon mainWeapon;

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

        PlayerInputManager.Instance.AttackEvent += TryToPerformAttack;
    }

    private void TryToPerformAttack()
    {
        if(mainWeapon && PlayerInputManager.Instance.IsAiming)
        {
            mainWeapon.PerformAttack();
        }
    }
}
