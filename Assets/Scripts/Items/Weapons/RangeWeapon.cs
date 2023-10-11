using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangeWeapon : MonoBehaviour, IWeapon, IReloadableWeapon
{
    protected abstract void Awake();

    public abstract void PerformAttack();

    protected abstract IEnumerator PerformShot();

    protected abstract void ShotLogic();

    protected abstract IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit);

    public abstract void PerformReload();

    public abstract void LoadMagazine();

    protected abstract TrailRenderer CreateTrail();

    public abstract void SetBulletStatus();
}
