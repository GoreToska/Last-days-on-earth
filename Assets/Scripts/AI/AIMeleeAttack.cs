using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeAttack : MonoBehaviour, IAILightAttack
{
    [SerializeField] private float _defaultDamage;
    private float _damage = 0;
    private MeleeDamageCollider _damageCollider;

    void Start()
    {
        _damageCollider = GetComponentInChildren<MeleeDamageCollider>();
        Debug.Log(_damageCollider);
    }

    public void StartMeleeAttack()
    {
        _damageCollider.EnableCollider(_damage);
    }

    public void EndMeleeAttack()
    {
        _damageCollider.DisableCollider();
    }

    public void PerformLightMeleeAttack(BaseAIAgent agent, float damage = 0)
    {
        SetDamage(damage);

        agent.isAttacking = true;
        agent.navMeshAgent.isStopped = true;
        agent.animator.SetTrigger("LightAttack");
    }

    public void PerformHeavyMeleeAttack(BaseAIAgent agent, float damage = 0)
    {
        SetDamage(damage);

        agent.isAttacking = true;
        agent.navMeshAgent.isStopped = true;
        agent.animator.SetTrigger("HeavyAttack");
    }

    private void SetDamage(float damage = 0)
    {
        if (damage == 0)
        {
            _damage = _defaultDamage;
        }
        else
        {
            _damage = damage;
        }
    }
}