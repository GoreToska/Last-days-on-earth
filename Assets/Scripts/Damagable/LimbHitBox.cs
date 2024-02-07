using PixelCrushers.DialogueSystem.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbHitBox : HitBox
{
    [SerializeField] private float _damageMultiplier = 0.25f;

    private IDamagable _damagableCharacter;

    protected override void Start()
    {
        _damagableCharacter = transform.root.GetComponent<IDamagable>();
    }

    public override void GetDamage(float damage, GameObject d)
    {
        _damagableCharacter.TakeDamage(damage * _damageMultiplier, d);
    }
}
