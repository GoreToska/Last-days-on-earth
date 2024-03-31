using UnityEngine;

public class LightSniperWeapon : RangeWeapon
{
	protected override void ShotLogic()
	{
		playerAnimationManager.PlayRifleHeavyShot();
		base.ShotLogic();
	}
}
