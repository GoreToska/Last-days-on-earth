using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHitBox : HitBox
{
    [SerializeField] private float damageMultiplier = 1f;

    private DamagableZombie damagableCharacter;

    protected override void Start()
    {
        damagableCharacter = transform.root.GetComponent<DamagableZombie>();
    }

    public override void GetDamage(float damage)
    {
        damagableCharacter.TakeDamage(damage * damageMultiplier);
        Debug.Log("Body");
    }
}
