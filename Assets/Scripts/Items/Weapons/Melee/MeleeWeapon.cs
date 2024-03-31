using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MeleeWeapon : MainWeapon
{
    [SerializeField] protected WeaponData weaponData;
    [Header("Prefab of this weapon for dropping it on ground")]
    [SerializeField] public GameObject itemPrefab;
    [Inject] protected PlayerAnimationManager playerAnimationManager;

    public override void PerformAttack()
    {
        Debug.Log("Melee attack");
        AttackLogic();
    }

    public override void PerformReload()
    {
        Debug.Log("Reload or something idk");
    }

    protected virtual void AttackLogic()
    {
        return;
    }
}
