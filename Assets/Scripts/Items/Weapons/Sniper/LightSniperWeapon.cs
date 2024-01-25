using UnityEngine;

public class LightSniperWeapon : RangeWeapon
{
    protected override void ShotLogic()
    {
        PlayerAnimationManager.Instance.PlayRifleHeavyShot();
        base.ShotLogic();
    }
}
