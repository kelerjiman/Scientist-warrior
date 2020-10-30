using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BodyPart : MonoBehaviour
{
    [FormerlySerializedAs("DefaultModel")] public SpriteRenderer ModelHolder;
    private Sprite defalutModel;
    public BodyPartProp Properties;

    private void Start()
    {
        Properties.Id = Random.Range(0, 10000);
        Properties.Name = Properties.Type.ToString();
        ModelHolder = GetComponentInChildren<SpriteRenderer>();
        defalutModel = ModelHolder.sprite;
    }
    public void ResetModel()
    {
        ModelHolder.sprite = defalutModel;
    }
    public void setModel(EquipableItem item)
    {
        ModelHolder.sprite = item.Model.GetComponent<SpriteRenderer>().sprite;
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
    EyeBrowR,
    EyeBrowL,
    EyeR,
    EyeL,
    Chest,
    Shoulder,
    HandR,
    HandL,
    GloveR,
    GloveL,
    Shield,
    Weapon,
    FootR,
    FootL,
    Shadow
}