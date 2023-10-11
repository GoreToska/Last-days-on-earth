using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DamagableZombie : MonoBehaviour, IDamagable
{
    [SerializeField] private float HP = 100f;
    private AIZombieAgent zombieAgent;

    private void Start()
    {
        zombieAgent = GetComponent<AIZombieAgent>();
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            zombieAgent.stateMachine.ChangeState(AIStateID.Dead);
        }
    }
}
