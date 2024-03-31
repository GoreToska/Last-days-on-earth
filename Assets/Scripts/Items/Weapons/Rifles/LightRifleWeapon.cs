public class LightRifleWeapon : RangeWeapon
{
    protected override void ShotLogic()
    {
        playerAnimationManager.PlayRifleLightShot();

        base.ShotLogic();
    }
}
