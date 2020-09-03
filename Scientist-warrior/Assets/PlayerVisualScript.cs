using System.Collections.Generic;
using UnityEngine;


public class PlayerVisualScript : MonoBehaviour
{
    public Transform BodyPartHolder;
    public List<BodyPart> playerBodyParts;
    public static PlayerVisualScript Instance;

    private void OnValidate()
    {
        if (playerBodyParts.Count <= 1)
            foreach (var BodyPart in BodyPartHolder.GetComponentsInChildren<BodyPart>())
            {
                playerBodyParts.Add(BodyPart);
            }
    }

    private void Start()
    {
        foreach (var BodyPart in BodyPartHolder.GetComponentsInChildren<BodyPart>())
            playerBodyParts.Add(BodyPart);

        Instance = this;
    }

    public void SetItemVisual(EquipableItem item)
    {
        foreach (var bodyPart in playerBodyParts)
        {
            if (bodyPart.Properties.Type == item.Properties.Type)
            {
                bodyPart.CurrentModel = Instantiate(item.Model,bodyPart.transform);
                bodyPart.CurrentModel.transform.position = bodyPart.defaultModel.transform.position;
                bodyPart.defaultModel.SetActive(false);
                return;
            }
        }
    }

    public void RemoveItemVisual(EquipableItem item)
    {
        foreach (var bodyPart in playerBodyParts)
        {
            if (bodyPart.Properties.Type == item.Properties.Type)
            {
                Destroy(bodyPart.CurrentModel);
                bodyPart.defaultModel.SetActive(true);
            }
        }
    }

    public bool HaveItem(EquipableItem item)
    {
        foreach (var bodyPart in playerBodyParts)
            if (bodyPart.Properties.Type == item.Properties.Type &&
                bodyPart.CurrentModel == item.Model)
                return true;

        return false;
    }
}