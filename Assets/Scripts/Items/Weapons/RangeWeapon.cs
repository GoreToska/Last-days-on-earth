using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class RangeWeapon : MonoBehaviour, IRangeWeapon
{
    [Header("Weapon options")]
    [SerializeField] protected WeaponData weaponData;
    [SerializeField] protected GameObject burrel;
    [SerializeField] protected ParticleSystem muzzleFlash;

    //[Header("Prefab of this weapon for dropping it on ground")]
    //[SerializeField] public GameObject itemPrefab;

    [SerializeField] protected int bullets = 0;

    protected ParticleSystem particleSystem;
    protected ObjectPool<TrailRenderer> trailRendererPool;

    private float _shotTimer = 0f;
    private float _currentRecoil = 0f;
    private float _recoilStop;
    private bool _canShoot = true;

    private void Update()
    {
        if (PlayerInputManager.Instance.isShooting)
            return;

        if (_shotTimer <= 0f)
        {
            if (_currentRecoil <= 0f)
                return;

            _currentRecoil -= weaponData.Recoil;
            return;
        }

        _shotTimer -= Time.deltaTime;
    }

    protected virtual void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        trailRendererPool = new ObjectPool<TrailRenderer>(CreateTrail);
        _recoilStop = weaponData.RecoilStopShot * weaponData.Recoil;
    }

    protected virtual IEnumerator ShotCoroutine()
    {
        if (weaponData.IsAuto)
        {
            while (PlayerInputManager.Instance.isShooting)
            {
                if (bullets == 0)
                {
                    // sound click of empty magazine
                    yield break;
                }

                ShotLogic();

                yield return new WaitForSeconds(60f / weaponData.FireRate);
            }

            yield break;
        }
        else
        {
            if (!_canShoot)
                yield break;

            if (bullets == 0)
            {
                // sound click of empty magazine
                yield break;
            }

            ShotLogic();

            _canShoot = false;
            yield return new WaitForSeconds(60f / weaponData.FireRate);
            _canShoot = true;

            yield break;
        }

    }

    protected virtual void ShotLogic()
    {
        bullets--;

        var (success, position) = PlayerInputManager.Instance.GetMousePosition();

        Vector3 direction = new Vector3(0, 0, burrel.transform.localPosition.z) * 100f;

        Vector3 recoiledPosition = position + new Vector3(
            Random.Range(-_currentRecoil, _currentRecoil),
            Random.Range(-_currentRecoil, _currentRecoil),
            0);

        Ray ray = new Ray(burrel.transform.position, recoiledPosition - burrel.transform.position);
        var raycast = Physics.Raycast(ray, out var hit, Mathf.Infinity, PlayerInputManager.Instance.aimMask);

        Debug.DrawRay(burrel.transform.position, recoiledPosition - burrel.transform.position, Color.blue, 2);
        
        if(hit.point == Vector3.zero)
        {
            StartCoroutine(PlayTrail(burrel.transform.position, recoiledPosition, hit));
        }
        else
        {
            StartCoroutine(PlayTrail(burrel.transform.position, hit.point, hit));
        }

        if (_currentRecoil <= _recoilStop)
        {
            _currentRecoil += weaponData.Recoil;
        }

        _shotTimer = weaponData.RecoilTime;
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

            if(hit.collider.tag == "Damagable")
            {
                hit.collider.GetComponent<HitBox>().GetDamage(weaponData.Damage);
            }
        }

        yield return new WaitForSeconds(weaponData.trailRenderer.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        trailRendererPool.Release(instance);
    }

    void IRangeWeapon.PerformReload(PlayerInventoryHolder playerInventory, PlayerAnimationManager playerAnimation)
    {
        int ammoToLoad = weaponData.MagazineSize - bullets;

        var actualAmount = playerInventory.RemoveFromInventory(weaponData.AmmoType, ammoToLoad);

        if (actualAmount != 0)
        {
            bullets += actualAmount;
            playerAnimation.PlayReloadAnimation(weaponData.reloadAnimation.name);
        }
    }

    void IRangeWeapon.PerformShot()
    {
        if (!PlayerInputManager.Instance.IsAiming)
        {
            return;
        }

        StartCoroutine(ShotCoroutine());
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
}
