using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName ="Weapon Data",menuName = "Epidemic/Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [SerializeField] public GameObject weaponModel;
    [SerializeField] public WeaponType weaponType;
    [SerializeField] public string weaponName;
    [SerializeField] public string weaponDescription;
    [SerializeField] public float damage;
    [SerializeField] public float fireRate;
    [SerializeField] public float recoil;
    [SerializeField] public float magazineSize;
    [SerializeField] public string reloadAnimationName;
    [SerializeField] public float shotVolumeRadius;
    [SerializeField] public bool isAuto;
    [SerializeField] public bool drawSight;
}

public enum WeaponType
{
    Rifle,
    Pistol,
    Shotgun
}
