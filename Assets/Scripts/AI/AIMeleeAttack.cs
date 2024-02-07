using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeAttack : MonoBehaviour, IAIAttack
{
    [SerializeField] private float _defaultDamage;
    private MeleeDamageCollider _damageCollider;

    void Start()
    {
        _damageCollider = GetComponentInChildren<MeleeDamageCollider>();
        Debug.Log(_damageCollider);
    }

    public void StartMeleeAttack()
    {
        _damageCollider.EnableCollider(_defaultDamage);
        Debug.Log("Enable");
    }

    public void EndMeleeAttack()
    {
        _damageCollider.DisableCollider();
    }

    public void PerformLightMeleeAttack(BaseAIAgent agent)
    {
        agent.isAttacking = true;
        agent.navMeshAgent.isStopped = true;
        agent.animator.SetTrigger("LightAttack");
    }
}