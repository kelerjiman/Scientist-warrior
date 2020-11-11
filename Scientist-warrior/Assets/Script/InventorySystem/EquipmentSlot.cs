using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class EquipmentSlot : ItemSlot
{
    public EquipmentType EquipmentType;
    public override bool CanRecieveItem(Item item)
    {
        if (item == null)
        {
            return true;
        }
        EquipableItem equipableItem = item as EquipableItem;
        //if (equipmentType == EquipmentType.MainHand)
        //{
        //    if (equipableItem != null && (equipableItem.EquipmentType == EquipmentType.MainHand ||
        //        equipableItem.EquipmentType == EquipmentType.Range))
        //        return true;
        //    else
        //        return false;
        //}
        if (EquipmentType == EquipmentType.Shield)
        {
            if (equipableItem != null && (
                equipableItem.EquipmentType == EquipmentType.Shield ||
                equipableItem.EquipmentType == EquipmentType.Range ||
                equipableItem.EquipmentType == EquipmentType.offHand
                ))
                return true;
            else
                return false;
        }
        if (EquipmentType == EquipmentType.MainHand)
        {
            if (equipableItem != null && (
                  equipableItem.EquipmentType == EquipmentType.MainHand ||
                  equipableItem.EquipmentType == EquipmentType.Staff    ||
                  equipableItem.EquipmentType == EquipmentType.TwoHand
                  ))
                return true;
            else
                return false;

        }
        else
            return equipableItem != null && equipableItem.EquipmentType == EquipmentType;
    }

    protected override void OnValidate()
    {
        gameObject.name = EquipmentType.ToString() + " Slot";
        base.OnValidate();
    }
}
