using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BodyPart : MonoBehaviour
{
    [FormerlySerializedAs("DefaultModel")] public GameObject defaultModel;
    public GameObject CurrentModel;
    public BodyPartProp Properties;

    private void Start()
    {
        Properties.Id = Random.Range(0, 10000);
        Properties.Name = Properties.Type.ToString();
        defaultModel = GetComponentInChildren<SpriteRenderer>().gameObject;
        CurrentModel = defaultModel;
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