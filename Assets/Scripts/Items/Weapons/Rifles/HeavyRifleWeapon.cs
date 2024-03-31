using UnityEngine;

public class HeavyRifleWeapon : RangeWeapon
{
    protected override void ShotLogic()
    {
        playerAnimationManager.PlayRifleMediumShot();
        base.ShotLogic();
    }
}
