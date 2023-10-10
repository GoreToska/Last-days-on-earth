using PixelCrushers.DialogueSystem.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbHitBox : HitBox
{
    [SerializeField] private float damageMultiplier = 0.25f;

    private DamagableCharacter damagableCharacter;

    protected override void Start()
    {
        damagableCharacter = transform.root.GetComponent<DamagableCharacter>();
    }

    public override void GetDamage(float damage)
    {
        damagableCharacter.TakeDamage(damage * damageMultiplier);
        Debug.Log("Limb");
    }
}
