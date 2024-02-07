using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHitBox : HitBox
{
    [SerializeField] private float _damageMultiplier = 1.5f;

    private IDamagable _damagableCharacter;

    protected override void Start()
    {
        _damagableCharacter = transform.root.GetComponent<IDamagable>();
    }

    public override void GetDamage(float damage, GameObject targetCausedDamage)
    {
        _damagableCharacter.TakeDamage(damage * _damageMultiplier, targetCausedDamage);
    }
}
