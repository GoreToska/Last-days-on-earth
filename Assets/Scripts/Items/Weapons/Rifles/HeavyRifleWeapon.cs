using UnityEngine;

public class HeavyRifleWeapon : RangeWeapon
{
    protected override void ShotLogic()
    {
        PlayerAnimationManager.Instance.PlayRifleMediumShot();
        base.ShotLogic();
    }
}
