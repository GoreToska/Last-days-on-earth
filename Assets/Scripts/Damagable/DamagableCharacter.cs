using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DamagableCharacter : MonoBehaviour, IDamagable
{
    [SerializeField] private float HP = 100f;

    private Ragdoll ragdoll;

    private void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        Debug.Log("Dead");

        ragdoll.EnableRagdoll();
        //Destroy(gameObject);
    }
}
