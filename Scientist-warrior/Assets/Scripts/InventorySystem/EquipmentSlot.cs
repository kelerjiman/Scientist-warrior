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
        return equipableItem != null && equipableItem.EquipmentType == EquipmentType;
    }

    protected override void OnValidate()
    {
        gameObject.name = EquipmentType.ToString() + " Slot";
        base.OnValidate();
    }
}
