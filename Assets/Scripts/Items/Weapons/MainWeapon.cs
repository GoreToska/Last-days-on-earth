using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class MainWeapon : RangeWeapon
{
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] protected GameObject burrel;
    [SerializeField] protected ParticleSystem muzzleFlash;

    [Header("Prefab of this weapon for dropping it on ground")]
    [SerializeField] public GameObject itemPrefab;

    [SerializeField] protected int bullets = 0;

    protected ParticleSystem particleSystem;
    protected ObjectPool<TrailRenderer> trailRendererPool;

    public StoredItem storedItem;

    protected override void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        trailRendererPool = new ObjectPool<TrailRenderer>(CreateTrail);
    }

    public override void PerformAttack()
    {
        if (!PlayerInputManager.Instance.isShooting)
        {
            return;
        }

        StartCoroutine(PerformShot());
    }

    protected override IEnumerator PerformShot()
    {
        if (weaponData.isAuto)
        {
            while (PlayerInputManager.Instance.isShooting)
            {
                if (bullets == 0)
                {
                    // sound click of empty magazine
                    yield break;
                }

                ShotLogic();

                yield return new WaitForSeconds(60f / weaponData.fireRate);
            }

            yield break;
        }
        else
        {
            if (bullets == 0)
            {
                // sound click of empty magazine
                yield break;
            }

            ShotLogic();
            PlayerInputManager.Instance.isShooting = false;
        }

    }

    protected override void ShotLogic()
    {
        bullets--;

        var (success, position) = PlayerInputManager.Instance.GetMousePosition();

        var ray = Physics.SphereCast(burrel.transform.position, 0.15f, burrel.transform.forward, out var hit, Mathf.Infinity, PlayerInputManager.Instance.aimMask);
        Debug.DrawRay(burrel.transform.position, hit.point, Color.green, 2);

        if (hit.point != Vector3.zero)
        {
            StartCoroutine(PlayTrail(burrel.transform.position, hit.point, hit));
        }
        else
        {
            StartCoroutine(PlayTrail(burrel.transform.position, position, hit));
        }

        if (ray && hit.collider.tag == "Damagable")
        {
            hit.collider.GetComponent<HitBox>().GetDamage(weaponData.damage);
        }
    }

    protected override IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
    {
        muzzleFlash.Play();
        TrailRenderer instance = trailRendererPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;

        yield return null;

        instance.emitting = true;

        float distance = Vector3.Distance(startPoint, endPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(startPoint, endPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance)));
            remainingDistance -= weaponData.trailRenderer.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = endPoint;

        if (hit.collider != null)
        {
            //  impact
        }

        yield return new WaitForSeconds(weaponData.trailRenderer.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        trailRendererPool.Release(instance);
    }

    public override void PerformReload()
    {
        Debug.Log("Reload");
    }

    public override void LoadMagazine()
    {
        HUDManager.Instance.UpdateBulletsStatus(bullets);
        Debug.Log("Loaded " + bullets);
    }

    protected override TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = weaponData.trailRenderer.Color;
        trail.material = weaponData.trailRenderer.Material;
        trail.widthCurve = weaponData.trailRenderer.widthCurve;
        trail.time = weaponData.trailRenderer.Duration;
        trail.minVertexDistance = weaponData.trailRenderer.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }

    public override void SetBulletStatus()
    {
    }
}
