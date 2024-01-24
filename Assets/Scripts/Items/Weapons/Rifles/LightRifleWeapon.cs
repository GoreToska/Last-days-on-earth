public class LightRifleWeapon : RangeWeapon
{
    protected override void ShotLogic()
    {
        PlayerAnimationManager.Instance.PlayRifleLightShot();
        base.ShotLogic();
    }
}
