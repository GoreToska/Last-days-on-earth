using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon, IReloadableWeapon
{
    [SerializeField] private WeaponData weaponData;

    public void PerformAttack()
    {

    }

    public void PerformReload()
    {
        
    }
}
