using UnityEngine;

public interface IDamagable
{
    public bool IsDead { get; }
    public void TakeDamage(float damage, GameObject targetCausedDamage);
}
