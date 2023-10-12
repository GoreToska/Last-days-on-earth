using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeAttack : MonoBehaviour
{
    [SerializeField] private float defaultDamage;
    private MeleeDamageCollider damageCollider;

    void Start()
    {
        damageCollider = GetComponentInChildren<MeleeDamageCollider>();
    }

    public void PerformMeleeAttack()
    {
        damageCollider.EnableCollider(defaultDamage);
    }

    public void EndMeleeAttack()
    {
        damageCollider.DisableCollider();
    }
}
