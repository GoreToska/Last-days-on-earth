using UnityEngine;

public abstract class MainWeapon : MonoBehaviour, IWeapon, IReloadableWeapon
{
    public abstract void PerformAttack();

    public abstract void PerformReload();
}