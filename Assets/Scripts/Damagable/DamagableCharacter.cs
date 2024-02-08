using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BaseAIAgent), typeof(Ragdoll))]
public class DamagableCharacter : MonoBehaviour
{
    [SerializeField] protected float _maxHP = 100f;
    [SerializeField] protected float _hp = 100f;

    public UnityAction<GameObject> OnTakeDamageEvent;

    protected Ragdoll _ragdoll;
    protected BaseAIAgent _agent;
    protected AIMeleeAttack _meleeAttack;
    protected bool _isDead = false;

    public bool IsDead { get => _isDead; }

    protected virtual void Awake()
    {
        _agent = GetComponent<BaseAIAgent>();
        _ragdoll = GetComponent<Ragdoll>();

        if(_agent is MeleeAIAgent)
        {
            _meleeAttack = GetComponent<AIMeleeAttack>();
        }

        OnTakeDamageEvent += DamageReaction;
    }

    public void TakeDamage(float damage, GameObject targetCausedDamage)
    {
        if (_isDead)
        {
            return;
        }

        CalculdateDamage(damage);
        
        if (!_isDead)
        {
            OnTakeDamageEvent?.Invoke(targetCausedDamage);
        }
    }

    protected virtual void DamageReaction(GameObject from)
    {
        _agent.AddTarget(from, 100);
        _agent.stateMachine.ChangeState(AIStateID.ChasePlayer);
    }

    protected virtual void CalculdateDamage(float damage)
    {
        _hp -= damage;

        _hp = Mathf.Clamp(_hp, 0, _maxHP);

        if (_hp <= 0)
        {
            _isDead = true;
            _ragdoll.EnableRagdoll();
            _agent.SetDeadState();

            if(_meleeAttack)
            {
                _meleeAttack.EndMeleeAttack();
            }

            return;
        }
    }
}
