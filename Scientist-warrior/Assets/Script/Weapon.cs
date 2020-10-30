using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon_", menuName = "Weapon")]
public class Weapon : EquipableItem
{
    private void OnValidate()
    {
        Properties.Type = BodyPartType.Weapon;
        if (projectile == null)
            EquipmentType = EquipmentType.MainHand;
        else
            EquipmentType = EquipmentType.Range;

    }
    public Projectile projectile;
    public AttackType attackType;
}
