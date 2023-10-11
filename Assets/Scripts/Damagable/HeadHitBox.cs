using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHitBox : HitBox
{
    [SerializeField] private float damageMultiplier = 1.5f;

    private DamagableZombie damagableCharacter;

    protected override void Start()
    {
        damagableCharacter = transform.root.GetComponent<DamagableZombie>();
    }

    public override void GetDamage(float damage)
    {
        damagableCharacter.TakeDamage(damage * damageMultiplier);
        Debug.Log("Head");
    }
}
