using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRifleWeapon : MainWeapon
{
    protected override void ShotLogic()
    {
        PlayerAnimationManager.Instance.PlayRifleLightShot();
        base.ShotLogic();
        HUDManager.Instance.UpdateBulletsStatus(bullets);
    }

    public override void PerformReload()
    {
        if (PlayerInventory.Instance.LightRifleAmmoCount > 0 && bullets < weaponData.magazineSize)
        {
            PlayerAnimationManager.Instance.PlayRifleReloadAnimation();
        }
    }

    public override void LoadMagazine()
    {
        int ammoToLoad = weaponData.magazineSize - bullets;


        if (PlayerInventory.Instance.LightRifleAmmoCount / ammoToLoad >= 1)
        {
            PlayerInventory.Instance.SubtractLightRifleAmmo(ammoToLoad);
            bullets += ammoToLoad;
        }
        else
        {
            ammoToLoad = PlayerInventory.Instance.LightRifleAmmoCount;
            PlayerInventory.Instance.SubtractLightRifleAmmo(ammoToLoad);
            bullets += ammoToLoad;
        }

        base.LoadMagazine();
    }

    public override void SetBulletStatus()
    {
        HUDManager.Instance.UpdateBulletsStatus(bullets);
    }
}
