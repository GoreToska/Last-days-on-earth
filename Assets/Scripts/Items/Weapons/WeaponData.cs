using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Epidemic/Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [SerializeField] public WeaponType WeaponType;
    [SerializeField] public InventoryItemData AmmoType;
    [SerializeField] public float Damage;
    [SerializeField] public float FireRate;
    [Range(0.001f, 0.1f)]
    [SerializeField] public float Recoil;
    [Range(0.1f, 1f)]
    [SerializeField] public float RecoilTime;
    [SerializeField] public AnimationClip reloadAnimation;
    [SerializeField] public int RecoilStopShot;
    [SerializeField] public int MagazineSize;
    [SerializeField] public float ShotVolumeRadius;
    [SerializeField] public bool IsAuto;
    [SerializeField] public bool DrawSight;
    [SerializeField] public BulletTrailData trailRenderer;
}

public enum WeaponType
{
    None,
    Range_Primary,
    Range_Secondary,
    Melee_Primary,
    Melee_Secondary,
}

