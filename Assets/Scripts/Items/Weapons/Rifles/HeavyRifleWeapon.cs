using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyRifleWeapon : MainWeapon
{
    protected override void ShotLogic()
    {
        PlayerAnimationManager.Instance.PlayRifleMediumShot();
        base.ShotLogic();
        HUDManager.Instance.UpdateBulletsStatus(bullets);
    }

    public override void PerformReload()
    {
        if (PlayerInventory.Instance.HeavyRifleAmmoCount > 0 && bullets < weaponData.magazineSize)
        {
            PlayerAnimationManager.Instance.PlayRifleReloadAnimation();
        }
    }

    public override void LoadMagazine()
    {
        int ammoToLoad = weaponData.magazineSize - bullets;


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

        base.LoadMagazine();
    }

    public override void SetBulletStatus()
    {
        HUDManager.Instance.UpdateBulletsStatus(bullets);
    }
}
