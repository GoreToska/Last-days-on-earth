using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeAttack : MonoBehaviour
{
    [SerializeField] private float _defaultDamage;
    
    private MeleeDamageCollider _damageCollider;

    void Start()
    {
        _damageCollider = GetComponentInChildren<MeleeDamageCollider>();
    }

    public void PerformMeleeAttack()
    {
        _damageCollider.EnableCollider(_defaultDamage);
    }

    public void EndMeleeAttack()
    {
        _damageCollider.DisableCollider();
    }
}
