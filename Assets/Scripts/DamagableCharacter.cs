using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableCharacter : MonoBehaviour, IDamagable
{
    [SerializeField] private float HP = 100f;

    public void TakeDamage(float damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            Debug.Log("Dead");
            Destroy(gameObject);
        }
    }
}
