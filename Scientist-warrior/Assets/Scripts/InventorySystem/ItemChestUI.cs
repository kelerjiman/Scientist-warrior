using System;
using System.Collections.Generic;
using Script;
using Script.InventorySystem;
using UnityEngine;
using UnityEngine.UI;

public class ItemChestUI : MonoBehaviour
{
    private ChestInWorld ActiveChest;
    public event Action<ItemSlot> ChestUiAddToInventoryEvent;
    public static ItemChestUI Instance;
    [SerializeField] private GameObject Parent;
    [SerializeField] private ItemSlot[] m_ItemSlots;

    private bool m_IsEmpty;
    [SerializeField] private Image DragableImage;
    private Inventory Inventory;
    private ItemSlot m_DraggedSlot;

    private void OnValidate()
    {
        if (Parent != null)
            m_ItemSlots = Parent.GetComponentsInChildren<ItemSlot>();
    }
    private void Awake()
    {
        foreach (var itemSlot in m_ItemSlots)
        {
            itemSlot.OnRightClickEvent += AddItemToInventory;
        }
        Instance = this;
    }

    private void Start()
    {
        DragableImage.enabled = false;
        Inventory = InventoryManager.Instance.inventory;
        Parent.SetActive(false);
    }
    public void AddItemToInventory(ItemSlot itemSlot)
    {
        var itemId = itemSlot.Item.Id;
        ChestUiAddToInventoryEvent?.Invoke(itemSlot);
        if (ActiveChest != null)
        {
            if (itemSlot.Amount <= 0)
                ActiveChest.itemProps.Remove(ActiveChest.itemProps.Find(item => item.item.Id == itemId));
            else
                ActiveChest.itemProps.Find(item => item.item.Id == itemId).amount = itemSlot.Amount;
        }
    }
    public void AddItem(ChestInWorld chest)
    {
        if (chest != null && chest.itemProps.Count > 0)
        {
            ActiveChest = chest;
            ActiveChest.DestroyEvent += ActiveChestOnDestroy;
            for (int i = 0; i < chest.itemProps.Count; i++)
            {
                m_ItemSlots[i].Item = chest.itemProps[i].item;
                m_ItemSlots[i].Amount = chest.itemProps[i].amount;
            }
            for (int i = chest.itemProps.Count; i < m_ItemSlots.Length; i++)
            {
                m_ItemSlots[i].Amount = 0;
            }

            Parent.SetActive(true);
        }
        else
        {
            foreach (var slot in m_ItemSlots)
            {
                slot.Amount = 0;
            }
            Parent.SetActive(false);
        }
    }
    private void ActiveChestOnDestroy(ChestInWorld chest)
    {
        ActiveChest = null;
        foreach (var item in m_ItemSlots)
        {
            item.Amount = 0;
        }
        Parent.SetActive(false);
    }
}