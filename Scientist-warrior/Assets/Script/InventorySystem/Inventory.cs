using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Script.QuestSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour, IItemContainer, IDropHandler
{
    [FormerlySerializedAs("Items")] [SerializeField]
    private List<Item> StartingItems;

    [SerializeField] private Transform itemsParent;
    [SerializeField] private ItemSlot[] itemslots;
    public event Action<ItemSlot> OnRightClickEvent;
    public event Action<ItemSlot> OnBeginDragEvent;
    public event Action<ItemSlot> OnDragEvent;
    public event Action<ItemSlot> OnEndDragEvent;
    public event Action<ItemSlot> OnDropEvent;
    public event Action OnDropVoidEvent;
    public event Action<Item> ItemQuestEvent;
    public event Action<Item> AddItemEvent;

    private void Start()
    {
        for (int i = 0; i < itemslots.Length; i++)
        {
            itemslots[i].OnRightClickEvent += OnRightClickEvent;
            itemslots[i].OnBeginDragEvent += OnBeginDragEvent;
            itemslots[i].OnDragEvent += OnDragEvent;
            itemslots[i].OnEndDragEvent += OnEndDragEvent;
            itemslots[i].OnDropEvent += OnDropEvent;
            itemslots[i].ItemQuestEvent += ItemQuestEvent;
        }

        SetStartingItems();
    }

    private void OnValidate()
    {
        if (itemsParent != null)
        {
            itemslots = itemsParent.GetComponentsInChildren<ItemSlot>();
        }

        SetStartingItems();
    }

    private void SetStartingItems()
    {
        int i = 0;
        for (; i < StartingItems.Count && i < itemslots.Length; i++)
        {
            itemslots[i].Item = StartingItems[i].GetCopy();
            itemslots[i].Amount = 1;
        }

        for (; i < itemslots.Length; i++)
        {
            itemslots[i].Item = null;
            itemslots[i].Amount = 0;
        }
    }

    public bool AddItem(Item item)
    {
        AddItemEvent?.Invoke(item);
        foreach (var itemSlot in itemslots)
            if (itemSlot.Item != null)
                if (itemSlot.Item.Id == item.Id)
                    if (itemSlot.Amount < itemSlot.Item.MaxStack)
                    {
                        itemSlot.Amount++;
                        return true;
                    }

        foreach (var itemSlot in itemslots)
            if (itemSlot.Item == null)
            {
                itemSlot.Item = item;
                itemSlot.Amount = 1;
                return true;
            }

        return false;
    }


    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < itemslots.Length; i++)
        {
            if (itemslots[i].Item == item)
            {
                itemslots[i].Amount--;
                return true;
            }
        }

        return false;
    }

    public bool RemoveItem(Item item, int amount)
    {
        if (ItemCount(item.Id) < amount)
            return false;
        for (int i = 0; i < itemslots.Length; i++)
        {
            if (itemslots[i].Item.Id == item.Id)
            {
                if (itemslots[i].Amount >= amount)
                {
                    itemslots[i].Amount -= amount;
                    return true;
                }

                amount -= itemslots[i].Amount;
                itemslots[i].Amount = 0;
            }
        }

        return false;
    }

    public Item RemoveItem(string ItemID)
    {
        for (int i = 0; i < itemslots.Length; i++)
        {
            Item item = itemslots[i].Item;
            if (item != null && item.Id == ItemID)
            {
                itemslots[i].Amount--;
                return item;
            }
        }

        return null;
    }

    public bool IsFull()
    {
        for (int i = 0; i < itemslots.Length; i++)
        {
            if (itemslots[i].Item == null)
            {
                return false;
            }
        }

        return true;
    }

    public bool ContainItem(Item item)
    {
        for (int i = 0; i < itemslots.Length; i++)
        {
            if (itemslots[i].Item == item)
                return true;
        }

        return false;
    }

    public int ItemCount(string itemId)
    {
        int count = 0;
        for (int i = 0; i < itemslots.Length; i++)
        {
            if (itemslots[i].Item != null && itemslots[i].Item.Id == itemId)
                count += itemslots[i].Amount;
        }

        return count;
    }

    public int EmptySlotCount()
    {
        int count = 0;
        foreach (var itemslot in itemslots)
        {
            if (itemslot.Item == null)
                count++;
        }

        return count;
    }

    public int ItemCount(Item item)
    {
        int count = 0;
        for (int i = 0; i < itemslots.Length; i++)
        {
            if (itemslots[i].Item == item)
                count += itemslots[i].Amount;
        }

        return count;
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDropVoidEvent?.Invoke();
    }
}