using System.Collections.Generic;
using Script.DialogSystem;
using UnityEngine;


public class PlayerVisualScript : MonoBehaviour
{
    public Transform BodyPartHolder;
    public List<BodyPart2D> playerBodyParts;
    public static PlayerVisualScript Instance;

    //private void OnValidate()
    //{
    //    if (playerBodyParts.Count <= 1)
    //        foreach (var BodyPart in BodyPartHolder.GetComponentsInChildren<BodyPart2D>())
    //        {
    //            playerBodyParts.Add(BodyPart);
    //        }
    //}

    private void Start()
    {
        foreach (var BodyPart in BodyPartHolder.GetComponentsInChildren<BodyPart2D>())
            playerBodyParts.Add(BodyPart);

        Instance = this;
    }

    public void SetItemVisual(EquipableItem item)
    {
        var MainBodyPart = playerBodyParts.Find(bp => bp.Properties.Type == item.BodyPartType);
        if (MainBodyPart != null)
            MainBodyPart.setModel(item.Model);
        if (item.ChainedItems == null || item.ChainedItems.Count == 0)
            return;
        foreach (var chainedItem in item.ChainedItems)
        {
            var BodyPart = playerBodyParts.Find(bp => bp.Properties.Type == chainedItem.BodyPartType);
            if (BodyPart != null)
                BodyPart.setModel(chainedItem.Model);
        }
        //foreach (var bodyPart in playerBodyParts)
        //{
        //    if (bodyPart.Properties.Type == item.BodyPartType)
        //    {
        //        bodyPart.setModel(item.Model);
        //        return;
        //    }
        //}
    }

    public void RemoveItemVisual(EquipableItem item)
    {
        var MainBodyPart = playerBodyParts.Find(bp => bp.Properties.Type == item.BodyPartType);
        if (MainBodyPart != null)
            MainBodyPart.ResetModel();
        if (item.ChainedItems == null || item.ChainedItems.Count == 0)
            return;
        foreach (var chainedItem in item.ChainedItems)
        {
            var BodyPart = playerBodyParts.Find(bp => bp.Properties.Type == chainedItem.BodyPartType);
            if (BodyPart != null)
                BodyPart.ResetModel();
        }
        //foreach (var bodyPart in playerBodyParts)
        //{
        //    if (bodyPart.Properties.Type == item.BodyPartType)
        //    {
        //        bodyPart.ResetModel();
        //    }
        //}
    }

    //public bool HaveItem(EquipableItem item)
    //{
    //    foreach (var bodyPart in playerBodyParts)
    //        if (bodyPart.Properties.Type == item.Properties.Type &&
    //            bodyPart.CurrentModel == item.Model)
    //            return true;

    //    return false;
    //}
}