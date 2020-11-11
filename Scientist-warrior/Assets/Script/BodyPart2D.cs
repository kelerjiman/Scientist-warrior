using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BodyPart2D : MonoBehaviour
{
    [FormerlySerializedAs("DefaultModel")] public SpriteRenderer ModelHolder;
    private Sprite defalutModel;
    //public BodyPartProp Properties;
    public BodyPartProp Properties;

    private void Start()
    {
        Properties.Id = UnityEngine.Random.Range(0, 10000);
        Properties.Name = Properties.Type.ToString();
        ModelHolder = GetComponentInChildren<SpriteRenderer>();
        defalutModel = ModelHolder.sprite;
    }
    public void ResetModel()
    {
        //2d
        ModelHolder.sprite = defalutModel;
    }
    public void setModel(Sprite model)
    {
        //2d
        ModelHolder.sprite = model;
    }
}

[Serializable]
public class BodyPartProp
{
    public string Name;
    public int Id;
    public BodyPartType Type;
}

public enum BodyPartType
{
    None,
    Cask,
    Head,
    Hat,
    EyeBrow,
    EyeBrowR,
    EyeBrowL,
    Eye,
    EyeR,
    EyeL,
    Chest,
    Cloak,
    ShoulderR,
    ShoulderL,
    ForearmR,
    ForearmL,
    HandR,
    HandL,
    GloveR,
    GloveL,
    Shield,
    WeaponL,
    Weapon,
    Lagan,
    UpLegR,
    UpLegL,
    FootR,
    FootL,
    Shadow,
    BowRiser,
    BowLimbU,
    BowLimbL
}