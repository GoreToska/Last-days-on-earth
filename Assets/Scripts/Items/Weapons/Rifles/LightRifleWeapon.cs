public class LightRifleWeapon : RangeWeapon
{
    protected override void ShotLogic()
    {
        PlayerAnimationManager.Instance.PlayRifleLightShot();
        base.ShotLogic();
        HUDManager.Instance.UpdateBulletsStatus(bullets);
    }

    public override void PerformReload()
    {
        
    }

    public override void LoadMagazine()
    {
        int ammoToLoad = weaponData.magazineSize - bullets;


       

        base.LoadMagazine();
    }

    public override void SetBulletStatus()
    {
        HUDManager.Instance.UpdateBulletsStatus(bullets);
    }

    public override void PerformAttack()
    {
        throw new System.NotImplementedException();
    }
}
