using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainWeapon : MonoBehaviour, IWeapon, IReloadableWeapon
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] protected GameObject burrel;

    [Header("Prefab of this weapon for dropping it on ground")]
    [SerializeField] public GameObject itemPrefab;

    [SerializeField] public int bullets = 0;

    public StoredItem storedItem;

    public void PerformAttack()
    {
        if (bullets == 0)
        {
            // sound click of empty magazine
            return;
        }

        bullets--;

        var (success, position) = PlayerInputManager.Instance.GetMousePosition();

        var ray = Physics.Raycast(burrel.transform.position, burrel.transform.forward, out var hit, Mathf.Infinity, PlayerInputManager.Instance.aimMask);
        //  Check ammo
        Debug.DrawRay(burrel.transform.position, burrel.transform.forward * 100f, Color.green, 2);

        if (ray && hit.collider.tag == "Damagable")
        {
            hit.collider.GetComponent<DamagableCharacter>().TakeDamage(weaponData.damage);
        }
    }

    public void PerformReload()
    {
        if (weaponData.ammoType == AmmoTypes.RifleHeavy)
        {
            if (PlayerInventory.Instance.HeavyRifleAmmoCount > 0 && bullets < weaponData.magazineSize)
            {
                int ammoToLoad = weaponData.magazineSize - bullets;
                //  perform animation
                if (PlayerInventory.Instance.HeavyRifleAmmoCount / ammoToLoad >= 1)
                {
                    PlayerInventory.Instance.SubtractHeavyRifleAmmo(ammoToLoad);
                    bullets += ammoToLoad;
                }
                else
                {
                    ammoToLoad = PlayerInventory.Instance.HeavyRifleAmmoCount;
                    PlayerInventory.Instance.SubtractHeavyRifleAmmo(ammoToLoad);
                    bullets += ammoToLoad;
                }

                Debug.Log("Loaded " + ammoToLoad);
            }

        }
    }
}
