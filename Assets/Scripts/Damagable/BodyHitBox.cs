using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHitBox : HitBox
{
    [SerializeField] private float _damageMultiplier = 1f;

    private IDamagable _damagableCharacter;

    protected override void Start()
    {
        _damagableCharacter = transform.root.GetComponent<IDamagable>();
    }

    public override void GetDamage(float damage, GameObject d = null)
    {
        _damagableCharacter.TakeDamage(damage * _damageMultiplier, d);
    }
}
