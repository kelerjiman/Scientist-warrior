using System;
using System.Collections.Generic;
using Script;
using Script.CharacterStatus;
using UnityEditor;
using UnityEngine;

public enum EquipmentType
{
    Helmet,
    Chest,
    Shield,
    Boot,
    MainHand,
    offHand,
    Range,
    Accessory1,
    Accessory2
}

[CreateAssetMenu]
public class EquipableItem : Item
{
    [Space]
    [Header("Equipable Item Setting")]
    public List<State> state;
    [Space] public EquipmentType EquipmentType;
    [Header("Visual Effect Attr")] public BodyPartProp Properties;
    public GameObject Model;
    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }
}