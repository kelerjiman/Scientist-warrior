using System;
using System.Collections.Generic;
using Script;
using UnityEngine;

[Serializable]
public class ItemProp
{
    public Item item;
    public int amount;
}

public class ChestInWorld : MonoBehaviour
{
    [SerializeField] private ItemChestUI chestUi;

    [SerializeField] public List<ItemProp> itemProps;

    private void Start()
    {
        chestUi = InventoryManager.Instance.ItemChestUI.GetComponent<ItemChestUI>();
        foreach (var itemProp in itemProps)
        {
            if (itemProp.item.MaxStack < itemProp.amount)
                itemProp.amount = itemProp.item.MaxStack;
        }
    }

    private void Update()
    {
        if (itemProps.Count <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!chestUi.gameObject.activeSelf)
                chestUi.gameObject.SetActive(true);
            chestUi.ActiveChest = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            chestUi.gameObject.SetActive(false);
    }
}