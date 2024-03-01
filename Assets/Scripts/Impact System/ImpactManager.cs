using GoreToska;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactManager : MonoBehaviour
{
	[field: HideInInspector] public static ImpactManager Instance { get; private set; }
	[field: SerializeField] public List<Impact> Impacts { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Debug.Log("Only one ImpactManager can be in this scene.");
			Destroy(gameObject);
		}
	}

	public void HandleImpact(GameObject hitObject, Vector3 hitPoint, Vector3 hitNormal, ImpactType type)
	{
		var impact = Impacts.Find(i => hitObject.CompareTag(i.SurfaceTag) && i.ImpactType == type);

		if (impact == null)
		{
			Debug.LogWarning("No impact");
		}
		else
		{
			PlayEffect(hitPoint, hitNormal, impact);
		}
	}

	public void PlayEffect(Vector3 hitPoint, Vector3 hitNormal, Impact impact)
	{
		ObjectPool particlePool = ObjectPool.CreateInstance(impact.EffectPrefab, 10);
		PoolableObject instance = particlePool.GetObject(hitPoint + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal));
		instance.transform.forward = hitNormal;

		// TODO: randomize rotation

		SFXManager.Instance.PlaySoundEffect(hitPoint, impact.SoundEffects[Random.Range(0, impact.SoundEffects.Count)], impact.MaxDistance);
	}
}
