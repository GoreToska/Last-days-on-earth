using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyRifleWeapon : RangeWeapon, IRangeWeapon
{
    protected override void ShotLogic()
    {
        PlayerAnimationManager.Instance.PlayRifleMediumShot();
        base.ShotLogic();
        //HUDManager.Instance.UpdateBulletsStatus(bullets);
    }

    public override void PerformReload()
    {
        //if (PlayerInventory.Instance.HeavyRifleAmmoCount > 0 && bullets < weaponData.magazineSize)
        //{
        //    PlayerAnimationManager.Instance.PlayRifleReloadAnimation();
        //}
    }

    public override void LoadMagazine()
    {
        int ammoToLoad = weaponData.magazineSize - bullets;


        //if (PlayerInventory.Instance.HeavyRifleAmmoCount / ammoToLoad >= 1)
        //{
        //    //PlayerInventory.Instance.SubtractHeavyRifleAmmo(ammoToLoad);
        //    //bullets += ammoToLoad;
        //}
        //else
        //{
        //    //ammoToLoad = PlayerInventory.Instance.HeavyRifleAmmoCount;
        //    //PlayerInventory.Instance.SubtractHeavyRifleAmmo(ammoToLoad);
        //    bullets += ammoToLoad;
        //}

        base.LoadMagazine();
    }

    public override void SetBulletStatus()
    {
        HUDManager.Instance.UpdateBulletsStatus(bullets);
    }

    public override void PerformAttack()
    {
        Debug.Log("Shot");
        throw new System.NotImplementedException();
    }

    void IRangeWeapon.PerformReload()
    {
        Debug.Log("Reload");
        throw new System.NotImplementedException();
    }

    void IRangeWeapon.PerformShot()
    {
        Debug.Log("Shot!");
        if (!PlayerInputManager.Instance.isShooting)
        {
            return;
        }

        StartCoroutine(PerformShot());
    }
}
