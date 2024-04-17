using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Weapon/WeaponSFX")]
public class WeaponSFXConfig : ScriptableObject
{
    [field:SerializeField] public AudioClip ShotSound {  get; private set; }
	[field: SerializeField] public AudioClip MagazineOutSound { get; private set; }
	[field: SerializeField] public AudioClip MagazineInSound { get; private set; }
	[field: SerializeField] public AudioClip ChamberSound { get; private set; }
	[field: SerializeField] public AudioClip EmptySound { get; private set; }
	[field:SerializeField] public int MaxShotSoundDistance {  get; private set; }
    [field:SerializeField] public int MaxReloadSoundDistance {  get; private set; }
    [field: SerializeField] public float Volume { get; private set; }
}
