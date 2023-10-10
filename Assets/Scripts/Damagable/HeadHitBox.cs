using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHitBox : HitBox
{
    [SerializeField] private float damageMultiplier = 1.5f;

    private DamagableCharacter damagableCharacter;

    protected override void Start()
    {
        damagableCharacter = transform.root.GetComponent<DamagableCharacter>();
    }

    public override void GetDamage(float damage)
    {
        damagableCharacter.TakeDamage(damage * damageMultiplier);
        Debug.Log("Head");
    }
}
