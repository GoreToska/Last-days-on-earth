using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Epidemic/Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [SerializeField] public WeaponType weaponType;
    [SerializeField] public AmmoTypes ammoType;
    [SerializeField] public float damage;
    [SerializeField] public float fireRate;
    [Range(0.001f, 0.1f)]
    [SerializeField] public float recoil;
    [Range(0.1f, 1f)]
    [SerializeField] public float recoilTime;
    [SerializeField] public int recoilStopShot;
    [SerializeField] public int magazineSize;
    [SerializeField] public string reloadAnimationName;
    [SerializeField] public float shotVolumeRadius;
    [SerializeField] public bool isAuto;
    [SerializeField] public bool drawSight;
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

