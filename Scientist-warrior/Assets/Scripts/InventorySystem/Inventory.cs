using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script;
using Script.QuestSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour, IItemContainer, IDropHandler
{
    [FormerlySerializedAs("Items")]
    [SerializeField]
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
    public Action<Item> AddItemEvent;
    public Action<Item> RemoveItemEvent;

    private void Start()
    {
        for (int i = 0; i < itemslots.Length; i++)
        {
            itemslots[i].Id = i;
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
            itemslots[i].Amount = itemslots[i].Amount;
        }

        for (; i < itemslots.Length; i++)
        {
            itemslots[i].Item = null;
            itemslots[i].Amount = 0;
        }
    }

    public bool AddItem(Item item)
    {
        foreach (var itemSlot in itemslots)
            if (itemSlot.Item != null)
                if (itemSlot.Item.Id == item.Id)
                    if (itemSlot.Amount < itemSlot.Item.MaxStack)
                    {
                        itemSlot.Amount++;
                        AddItemEvent?.Invoke(item);
                        ItemQuestEvent?.Invoke(item);
                        return true;
                    }

        foreach (var itemSlot in itemslots)
            if (itemSlot.Item == null)
            {
                itemSlot.Item = item;
                itemSlot.Amount = 1;
                AddItemEvent?.Invoke(item);
                ItemQuestEvent?.Invoke(item);
                return true;
            }

        //        AddItemEvent?.Invoke(item);
        return false;
    }
    public bool AddItem(Item item, int amount, out int returnedAmount)
    {
        returnedAmount = 0;
        foreach (var itemSlot in itemslots)
        {

            if (itemSlot.Item != null)
                if (itemSlot.Item.Id == item.Id)
                    if (itemSlot.Amount < itemSlot.Item.MaxStack)
                    {
                        int tempAmount = itemSlot.Item.MaxStack - (itemSlot.Amount + amount);
                        if (tempAmount >= 0)
                        {
                            itemSlot.Amount += amount;
                            ItemQuestEvent?.Invoke(item);
                            return true;
                        }
                        else
                        {
                            returnedAmount = (amount + itemSlot.Amount) - itemSlot.Item.MaxStack;
                            itemSlot.Amount = itemSlot.Item.MaxStack;
                            return AddItem(item, returnedAmount, out returnedAmount);
                        }
                    }
        }

        foreach (var itemSlot in itemslots)
            if (itemSlot.Item == null)
            {
                itemSlot.Item = item;
                itemSlot.Amount = amount;
                ItemQuestEvent?.Invoke(item);
                returnedAmount = 0;
                return true;
            }
        returnedAmount = amount;
        //        AddItemEvent?.Invoke(item);
        return false;
    }
    public bool AddItem(Item item, int amount)
    {
        foreach (var itemSlot in itemslots)
            if (itemSlot.Item != null)
                if (itemSlot.Item.Id == item.Id)
                    if (itemSlot.Amount < itemSlot.Item.MaxStack)
                    {
                        int tempAmount = itemSlot.Item.MaxStack - (itemSlot.Amount + amount);
                        if (tempAmount >= 0)
                        {
                            itemSlot.Amount += amount;
                            ItemQuestEvent?.Invoke(item);
                            return true;
                        }
                        else
                        {
                            var returnedAmount = itemSlot.Item.MaxStack - (amount + itemSlot.Amount);
                            itemSlot.Amount = itemSlot.Item.MaxStack;
                            return AddItem(item, returnedAmount);
                        }
                    }

        foreach (var itemSlot in itemslots)
            if (itemSlot.Item == null)
            {
                itemSlot.Item = item;
                itemSlot.Amount = amount;
                ItemQuestEvent?.Invoke(item);
                return true;
            }

        //        AddItemEvent?.Invoke(item);
        return false;
    }
    public List<Item> AddItem(List<Item> items)
    {
        int counter = 0;
        var TempItems = new List<Item>();
        if (items != null)
            foreach (var item in items)
            {
                if (item != null)
                {

                    foreach (var itemSlot in itemslots)
                        if (itemSlot.Item != null)
                            if (itemSlot.Item.Id == item.Id)
                                if (itemSlot.Amount < itemSlot.Item.MaxStack)
                                {
                                    itemSlot.Amount++;
                                    AddItemEvent?.Invoke(item);
                                    ItemQuestEvent?.Invoke(item);
                                    counter++;
                                    items.Remove(item);
                                    break;
                                }

                    foreach (var itemSlot in itemslots)
                        if (itemSlot.Item == null)
                        {
                            itemSlot.Item = item;
                            itemSlot.Amount = 1;
                            AddItemEvent?.Invoke(item);
                            ItemQuestEvent?.Invoke(item);
                            counter++;
                            items.Remove(item);
                            break;
                        }
                }
            }
        if (items != null && items.Count > 0)
        {
            foreach (var item in items)
            {
                if (item != null)
                {
                    TempItems.Add(item);
                }
            }
            return TempItems;
        }
        else
        {
            return null;
        }
    }
    public void AddItem(Item item, int ItemSlotID, int amount)
    {
        var target = itemslots.FirstOrDefault(s => s.Id == ItemSlotID);
        if (target != null)
        {
            target.Item = item;
            target.Amount = amount;
        }
        ItemQuestEvent?.Invoke(item);

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
            if (itemslots[i].Item != null && itemslots[i].Item.Id == item.Id)
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
    public bool RemoveItem(int itemSlotId, int amount)
    {
        var target = itemslots.FirstOrDefault(s => s.Id == itemSlotId);
        if (target != null && target.Item != null)
        {
            target.Amount -= amount;
            return true;
        }
        return false;
    }
    public bool RemoveItem(string itemId, int amount)
    {
        var target = itemslots.FirstOrDefault(s => s.Item.Id == itemId);
        if (target != null && target.Item != null)
        {
            target.Amount -= amount;
            return true;
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