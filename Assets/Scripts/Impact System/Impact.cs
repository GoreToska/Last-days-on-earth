using GoreToska;
using UnityEngine;

[CreateAssetMenu(fileName = "Impact", menuName = "Impact System/Impact")]
public class Impact : ScriptableObject
{
    [field: SerializeField] public string SurfaceTag { get; private set; }
    [field: SerializeField] public ImpactType ImpactType { get; private set; }
    [field: SerializeField] public PoolableParticle EffectPrefab { get; private set; }
    [field: SerializeField] public AudioClip SoundEffect { get; private set; }
    
    [SerializeField] public int MinDistance = 1;
    [SerializeField] public int MaxDistance = 5;
}