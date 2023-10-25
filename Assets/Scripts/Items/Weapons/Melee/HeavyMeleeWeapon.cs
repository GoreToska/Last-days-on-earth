using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyMeleeWeapon : MeleeWeapon
{
    protected override void AttackLogic()
    {
        PlayerAnimationManager.Instance.PlayHeavyAttackAnimation();
        base.AttackLogic();
    }
}
