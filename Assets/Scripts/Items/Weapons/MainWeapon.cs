using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class MainWeapon : MonoBehaviour, IWeapon, IReloadableWeapon
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private GameObject burrel;
    [SerializeField] private ParticleSystem muzzleFlash;

    [Header("Prefab of this weapon for dropping it on ground")]
    [SerializeField] public GameObject itemPrefab;

    [SerializeField] public int bullets = 0;

    private ParticleSystem particleSystem;
    private ObjectPool<TrailRenderer> trailRendererPool;

    public StoredItem storedItem;

    private void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        trailRendererPool = new ObjectPool<TrailRenderer>(CreateTrail);
    }

    public void PerformAttack()
    {
        if (!PlayerInputManager.Instance.isShooting)
        {
            return;
        }

        StartCoroutine(PerformShot());
    }

    private IEnumerator PerformShot()
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

    private void ShotLogic()
    {
        bullets--;
        PlayerAnimationManager.Instance.PlayRifleMediumShot();

        #region Ammo Status Update (refactor)
        if (weaponData.ammoType == AmmoTypes.RifleLight)
        {
            HUDManager.Instance.UpdateBulletsStatus(bullets);
        }

        if (weaponData.ammoType == AmmoTypes.RifleHeavy)
        {
            HUDManager.Instance.UpdateBulletsStatus(bullets);
        }

        if (weaponData.ammoType == AmmoTypes.Sniper)
        {
            HUDManager.Instance.UpdateBulletsStatus(bullets);
        }

        if (weaponData.ammoType == AmmoTypes.Shotgun)
        {
            HUDManager.Instance.UpdateBulletsStatus(bullets);
        }
        #endregion

        var (success, position) = PlayerInputManager.Instance.GetMousePosition();

        var ray = Physics.Raycast(burrel.transform.position, burrel.transform.forward, out var hit, Mathf.Infinity, PlayerInputManager.Instance.aimMask);
        Debug.DrawRay(burrel.transform.position, burrel.transform.forward * 100f, Color.green, 2);

        StartCoroutine(PlayTrail(burrel.transform.position, hit.point, hit));

        if (ray && hit.collider.tag == "Damagable")
        {
            hit.collider.GetComponent<DamagableCharacter>().TakeDamage(weaponData.damage);
        }
    }

    private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
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

    public void PerformReload()
    {
        if (weaponData.ammoType == AmmoTypes.RifleHeavy)
        {
            if (PlayerInventory.Instance.HeavyRifleAmmoCount > 0 && bullets < weaponData.magazineSize)
            {
                PlayerAnimationManager.Instance.PlayRifleReloadAnimation();
            }
        }
    }

    public void LoadMagazine()
    {
        int ammoToLoad = weaponData.magazineSize - bullets;


        if (PlayerInventory.Instance.HeavyRifleAmmoCount / ammoToLoad >= 1)
        {
            PlayerInventory.Instance.SubtractHeavyRifleAmmo(ammoToLoad);
            bullets += ammoToLoad;
        }
        else
        {
            ammoToLoad = PlayerInventory.Instance.HeavyRifleAmmoCount;
            PlayerInventory.Instance.SubtractHeavyRifleAmmo(ammoToLoad);
            bullets += ammoToLoad;
        }

        Debug.Log("Loaded " + ammoToLoad);
        HUDManager.Instance.UpdateBulletsStatus(bullets);
    }

    private TrailRenderer CreateTrail()
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

    public void SetBulletStatus()
    {
        #region Ammo Status Update (refactor)
        if (weaponData.ammoType == AmmoTypes.RifleLight)
        {
            HUDManager.Instance.UpdateBulletsStatus(bullets);
        }

        if (weaponData.ammoType == AmmoTypes.RifleHeavy)
        {
            HUDManager.Instance.UpdateBulletsStatus(bullets);
        }

        if (weaponData.ammoType == AmmoTypes.Sniper)
        {
            HUDManager.Instance.UpdateBulletsStatus(bullets);
        }

        if (weaponData.ammoType == AmmoTypes.Shotgun)
        {
            HUDManager.Instance.UpdateBulletsStatus(bullets);
        }
        #endregion
    }
}
