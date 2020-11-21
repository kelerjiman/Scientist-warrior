using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpriteResorter : MonoBehaviour
{

    [SerializeField] Transform parent;
    [SerializeField] SpriteRenderer[] part;
    [SerializeField] int[] sortOrder;
    public LayerType layerType;
    Vector2 defaultPos = Vector2.zero;
    //private void OnValidate()
    //{
    //    part = GetComponentsInChildren<SpriteRenderer>();
    //    sortOrder = new int[part.Length];
    //    defaultPos = parent.position;
    //    if (part != null)
    //        for (int i = 0; i < part.Length; i++)
    //        {
    //            part[i].sortingLayerID = SortingLayer.NameToID("Object");
    //            sortOrder[i] = part[i].sortingOrder;
    //            part[i].sortingOrder = sortOrder[i] + (int)parent.position.y * -1;
    //        }
    //}
    private void Start()
    {
        part = GetComponentsInChildren<SpriteRenderer>();
        sortOrder = new int[part.Length];
        defaultPos = parent.position;
        if (part != null)
            for (int i = 0; i < part.Length; i++)
            {
                part[i].sortingLayerID = SortingLayer.NameToID(layerType.ToString()) ;
                sortOrder[i] = part[i].sortingOrder;
                part[i].sortingOrder = sortOrder[i] + (int)parent.position.y * -1;
            }
    }
    private void Update()
    {
        if (parent!=null && Mathf.Abs(parent.position.y - defaultPos.y) > 0)
        {
            for (int i = 0; i < part.Length; i++)
            {
                part[i].sortingOrder = sortOrder[i] + (int)parent.localPosition.y * -1;
                defaultPos = parent.position;
            }
        }
    }
    public void ChangeParent(Transform NewParent)
    {
        parent = NewParent;
    }
    public void ChangeSortingLayer(LayerType type)
    {
        for (int i = 0; i < part.Length; i++)
        {
            part[i].sortingLayerID = SortingLayer.NameToID(type.ToString());
        }
    }
    public enum LayerType
    {
        Base,
        Object,
        Front,
        Behind
    }
}
