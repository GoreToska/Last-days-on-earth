using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWeapon : MonoBehaviour, IWeapon, IReloadableWeapon
{
    [SerializeField] private WeaponData weaponData;

    public void PerformAttack()
    {
        var ( success, position) = PlayerInputManager.Instance.GetMousePosition();

        //  Check ammo
        var hit = PlayerInputManager.Instance.hitInfo.collider;

        if (hit.tag == "Damagable")
        {
            hit.GetComponent<DamagableCharacter>().TakeDamage(weaponData.damage);
        }
    }

    public void PerformReload()
    {
        
    }
}
