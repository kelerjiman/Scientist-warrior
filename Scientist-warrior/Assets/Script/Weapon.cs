using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon_", menuName = "Weapon")]
public class Weapon : EquipableItem
{
    private void OnValidate()
    {
        
        if (projectile == null)
        {
            BodyPartType = BodyPartType.Weapon;
            EquipmentType = EquipmentType.MainHand;
        }
        else
        {
            BodyPartType = BodyPartType.Shield;
            EquipmentType = EquipmentType.Range;
        }

    }
    public Projectile projectile;
    public AttackType attackType;
}
