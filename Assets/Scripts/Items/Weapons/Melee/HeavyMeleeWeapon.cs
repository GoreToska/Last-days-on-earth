using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyMeleeWeapon : MeleeWeapon
{
    protected override void AttackLogic()
    {
        playerAnimationManager.PlayHeavyAttackAnimation();
        base.AttackLogic();
    }
}
