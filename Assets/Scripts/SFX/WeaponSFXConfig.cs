using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Weapon/WeaponSFX")]
public class WeaponSFXConfig : ScriptableObject
{
    [field:SerializeField] public AudioClip ShotSound {  get; private set; }
    [field: SerializeField] public AudioClip ReloadSound { get; private set; }
    [field:SerializeField] public int MaxShotSoundDistance {  get; private set; }
    [field:SerializeField] public int MaxReloadSoundDistance {  get; private set; }
    [field: SerializeField] public float Volume { get; private set; }
}
