using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class RangeWeapon : MainWeapon
{
    [Header("Weapon options")]
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] protected GameObject burrel;
    [SerializeField] protected ParticleSystem muzzleFlash;

    [Header("Prefab of this weapon for dropping it on ground")]
    [SerializeField] public GameObject itemPrefab;

    [SerializeField] protected int bullets = 0;

    protected ParticleSystem particleSystem;
    protected ObjectPool<TrailRenderer> trailRendererPool;

    private float shotTimer = 0f;
    private float currentRecoil = 0f;
    private float recoilStop;

    private void Update()
    {
        if (PlayerInputManager.Instance.isShooting)
            return;

        if (shotTimer <= 0f)
        {
            if (currentRecoil <= 0f)
                return;

            currentRecoil -= weaponData.recoil;
            return;
        }

        shotTimer -= Time.deltaTime;
    }

    protected virtual void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        trailRendererPool = new ObjectPool<TrailRenderer>(CreateTrail);
        recoilStop = weaponData.recoilStopShot * weaponData.recoil;
    }

    protected virtual IEnumerator PerformShot()
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
            yield return null;
        }

    }

    protected virtual void ShotLogic()
    {
        bullets--;

        var (success, position) = PlayerInputManager.Instance.GetMousePosition();

        Vector3 direction = new Vector3(0, 0, burrel.transform.localPosition.z) * 100f;

        Vector3 recoiledPosition = position + new Vector3(
            Random.Range(-currentRecoil, currentRecoil),
            Random.Range(-currentRecoil, currentRecoil),
            Random.Range(-currentRecoil, currentRecoil));

        Ray ray = new Ray(burrel.transform.position, recoiledPosition - burrel.transform.position);
        var sphere = Physics.SphereCast(ray, 0.15f, out var hit, Mathf.Infinity, PlayerInputManager.Instance.aimMask);

        Debug.DrawRay(burrel.transform.position, recoiledPosition - burrel.transform.position, Color.blue, 2);
        StartCoroutine(PlayTrail(burrel.transform.position, recoiledPosition, hit));

        if (sphere && hit.collider.tag == "Damagable")
        {
            hit.collider.GetComponent<HitBox>().GetDamage(weaponData.damage);
            Debug.Log("Damagable");
        }

        if (currentRecoil <= recoilStop)
        {
            currentRecoil += weaponData.recoil;
        }

        shotTimer = weaponData.recoilTime;
    }

    protected virtual IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
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

    public virtual void LoadMagazine()
    {
        HUDManager.Instance.UpdateBulletsStatus(bullets);
        Debug.Log("Loaded " + bullets);
    }

    protected virtual TrailRenderer CreateTrail()
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

    public abstract void SetBulletStatus();
}
