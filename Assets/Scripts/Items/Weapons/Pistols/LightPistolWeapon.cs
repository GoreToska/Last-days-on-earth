using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPistolWeapon : RangeWeapon
{
    protected override void ShotLogic()
    {
        PlayerAnimationManager.Instance.PlayPistolMediumShot();

        base.ShotLogic();
    }
}
