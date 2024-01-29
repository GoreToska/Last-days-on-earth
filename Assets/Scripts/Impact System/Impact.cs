using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Impact", menuName = "Impact System/Impact")]
public class Impact : ScriptableObject
{
    [field: SerializeField] public string SurfaceTag { get; private set; }
    [field: SerializeField] public ImpactType ImpactType { get; private set; }
    [field: SerializeField] public PoolableParticle EffectPrefab { get; private set; }
}