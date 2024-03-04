using UnityEngine;
using UnityEngine.Events;

public interface IDamagable
{
    public bool IsDead { get; }
    public void TakeDamage(float damage, GameObject targetCausedDamage);

    public event UnityAction OnDeath;
}
