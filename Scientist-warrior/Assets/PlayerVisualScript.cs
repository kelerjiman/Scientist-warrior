using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
[Serializable]
public class PlayerBodyPart
{
    public string Name;
    [SerializeField] Sprite m_Sprite;

    public Sprite Sprite
    {
        get { return m_Sprite; }
        set
        {
            m_Sprite = value;
            if (value != null)
            {
                PartHodler.sprite = value;
            }
        }
    }

    public SpriteRenderer PartHodler;

}
public class PlayerVisualScript : MonoBehaviour
{
    public List<PlayerBodyPart> playerBodyParts;
    public static PlayerVisualScript Instance;

    private void Start()
    {
        Instance = this;
    }

    public void SetItemVisual(string name,Sprite sprite)
    {
        foreach (var bodyPart in playerBodyParts)
        {
            if (bodyPart.Name == name)
            {
                bodyPart.Sprite = sprite;
                return;
            }
        }
    }

    [CanBeNull]
    public Sprite GetItemVisual(string name)
    {
        foreach (var bodyPart in playerBodyParts)
        {
            if (bodyPart.Name == name)
                return bodyPart.Sprite;
        }

        return null;
    }
}
