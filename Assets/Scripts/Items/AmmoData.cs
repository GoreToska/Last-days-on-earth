using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ammo Data", menuName = "Epidemic/Data/Ammo Data")]
public class AmmoData : ItemData
{
    public AmmoTypes ammoType;
    public int maxStack = 90;
}


public enum AmmoTypes
{
    RifleLight,
    RifleHeavy,
    Pistol,
    Sniper,
    Shotgun,
    None
}
