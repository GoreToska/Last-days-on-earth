using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ImpactManager : MonoBehaviour
{
    [field: HideInInspector] public static ImpactManager Instance { get; private set; }

    [field: SerializeField] public Impact DefaultImpact { get; private set; }

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
        var impact = Impacts.Find(i => hitObject.CompareTag(i.SurfaceTag));

        if (impact == null)
        {
            PlayEffect(hitPoint, hitNormal,DefaultImpact);
        }
        else
        {
            PlayEffect(hitPoint, hitNormal, impact);
        }
    }

    public void PlayEffect(Vector3 hitPoint, Vector3 hitNormal, Impact impact)
    {
        ParticlePool particlePool = ParticlePool.CreateInstance(impact.EffectPrefab, 10);
        PoolableParticle instance = particlePool.GetObject(hitPoint + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal));
        instance.transform.forward = hitNormal;

        // randomize rotation
    }

    private void PlaySound()
    {

    }
}
