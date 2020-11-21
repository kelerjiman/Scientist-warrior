using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon_", menuName = "Weapon")]
public class Weapon : EquipableItem
{

    public enum WeaponType
    {
        MainHand,
        OffHand,
        TwoHand,
        Bow,
        Staff
    }
    private void OnValidate()
    {
        if (weaponType == WeaponType.OffHand || weaponType == WeaponType.Bow)
            EquipmentType = EquipmentType.Shield;
        else
            EquipmentType = EquipmentType.MainHand;

    }
    [Space(5)]
    [Header("Weapon Properties")]
    public WeaponType weaponType;
    public Projectile projectile;
    public AttackType attackType;
}
