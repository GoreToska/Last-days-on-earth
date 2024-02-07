using UnityEngine;

public abstract class HitBox : MonoBehaviour
{
    protected abstract void Start();
    public abstract void GetDamage(float damage, GameObject targetCausedDamage = null);
}
