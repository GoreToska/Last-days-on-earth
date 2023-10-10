using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHitBox : HitBox
{
    [SerializeField] private float damageMultiplier = 1f;

    private DamagableCharacter damagableCharacter;

    protected override void Start()
    {
        damagableCharacter = transform.root.GetComponent<DamagableCharacter>();
    }

    public override void GetDamage(float damage)
    {
        damagableCharacter.TakeDamage(damage * damageMultiplier);
        Debug.Log("Body");
    }
}
