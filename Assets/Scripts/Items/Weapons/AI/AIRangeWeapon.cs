using GoreToska;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AIRangeWeapon : MonoBehaviour, IAIRangeWeapon
{
	[Header("Weapon options")]
	[SerializeField] protected WeaponData weaponData;
	[SerializeField] protected GameObject burrel;
	[SerializeField] protected ParticleSystem muzzleFlash;
	[SerializeField] protected int bullets = 0;

	int IAIRangeWeapon.Bullets { get => bullets; set => bullets = value; }

	protected ParticleSystem particleSystem;
	protected ObjectPool<TrailRenderer> trailRendererPool;
	protected DamagableCharacter character;

	private float _damage = 10f;
	private float _shotTimer = 0f;
	private float _currentRecoil = 0f;
	private float _recoilStop;
	private bool _canShoot = true;
	private bool _isShooting = false;

	private void Update()
	{
		if (_isShooting)
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
		character = transform.root.GetComponent<DamagableCharacter>();
	}

	public void Attack(GameObject target, AIAnimation animator, float damage = 10)
	{
		// TODO: wait some time before shot
		_damage = damage;
		StartCoroutine(ShotCoroutine(target, animator));
	}

	public void Reload()
	{
		bullets = weaponData.MagazineSize;

		// play anim
	}

	protected virtual IEnumerator ShotCoroutine(GameObject target, AIAnimation animator)
	{
		if (weaponData.IsAuto)
		{
			for (int i = 0; i < 3; i++)
			{
				if (bullets == 0)
				{
					// sound click of empty magazine
					yield break;
				}

				if(character.IsDead)
				{
					yield break;
				}

				ShotLogic(target);
				animator.PlayMediumRifleShot();

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

			ShotLogic(target);

			_canShoot = false;
			yield return new WaitForSeconds(60f / weaponData.FireRate);
			_canShoot = true;

			yield break;
		}

	}

	protected virtual void ShotLogic(GameObject target)
	{
		bullets--;
		transform.root.LookAt(target.transform, Vector3.up);

		SFXManager.Instance.PlaySoundEffect(burrel.transform.position, weaponData.WeaponSFXConfig.ShotSound, weaponData.WeaponSFXConfig.MaxShotSoundDistance);

		Vector3 recoiledPosition = target.transform.position + new Vector3(
			Random.Range(-_currentRecoil, _currentRecoil),
			Random.Range(-_currentRecoil, _currentRecoil),
			0);

		Ray ray = new Ray(burrel.transform.position, recoiledPosition - burrel.transform.position);
		var raycast = Physics.Raycast(ray, out var hit, Mathf.Infinity);

		//Debug
		Debug.DrawRay(burrel.transform.position, recoiledPosition - burrel.transform.position, Color.yellow, 2);

		if (hit.point == Vector3.zero)
		{
			//StartCoroutine(PlayTrail(burrel.transform.position, recoiledPosition, hit));
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
			remainingDistance -= weaponData.TrailRenderer.SimulationSpeed * Time.deltaTime;

			yield return null;
		}

		instance.transform.position = endPoint;

		if (hit.collider != null)
		{
			//  impact
			ImpactManager.Instance.HandleImpact(hit.transform.gameObject, hit.point, hit.normal, ImpactType.Shot);

			if (hit.collider.tag == "Damagable" && hit.transform.gameObject.layer != this.transform.root.gameObject.layer)
			{
				hit.collider.GetComponent<HitBox>().GetDamage(_damage, transform.root.gameObject);
			}
			else if (hit.transform.root.tag == "Player")
			{
				hit.transform.root.GetComponent<IDamagable>().TakeDamage(_damage, transform.root.gameObject);
			}
		}

		yield return new WaitForSeconds(weaponData.TrailRenderer.Duration);
		yield return null;

		instance.emitting = false;
		instance.gameObject.SetActive(false);
		trailRendererPool.Release(instance);
	}

	protected virtual TrailRenderer CreateTrail()
	{
		GameObject instance = new GameObject("Bullet Trail");
		TrailRenderer trail = instance.AddComponent<TrailRenderer>();
		trail.colorGradient = weaponData.TrailRenderer.Color;
		trail.material = weaponData.TrailRenderer.Material;
		trail.widthCurve = weaponData.TrailRenderer.widthCurve;
		trail.time = weaponData.TrailRenderer.Duration;
		trail.minVertexDistance = weaponData.TrailRenderer.MinVertexDistance;

		trail.emitting = false;
		trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

		return trail;
	}
}
