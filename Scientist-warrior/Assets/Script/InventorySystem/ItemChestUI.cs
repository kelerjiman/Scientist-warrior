using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemChestUI : MonoBehaviour
{
    private ChestInWorld m_ActiveChest;

    public ChestInWorld ActiveChest
    {
        get { return m_ActiveChest; }
        set
        {
            m_ActiveChest = value;
            for (int i = 0; i < m_ActiveChest.itemProps.Count; i++)
            {
                m_ItemSlots[i].Item = m_ActiveChest.itemProps[i].item;
                m_ItemSlots[i].Amount = m_ActiveChest.itemProps[i].amount;
                m_ItemSlots[i].OnRightClickEvent -= AddToInventory;
                m_ItemSlots[i].OnRightClickEvent += AddToInventory;
            }

            for (int i = m_ActiveChest.itemProps.Count; i < m_ItemSlots.Length; i++)
            {
                m_ItemSlots[i].Item = null;
            }
        }
    }

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

    private void Start()
    {
        DragableImage.enabled = false;
        Inventory = InventoryManager.Instance.inventory;
    }

    private void Update()
    {
        if (m_ActiveChest.itemProps.Count <= 0 && !m_IsEmpty)
        {
            m_IsEmpty = true;
            gameObject.SetActive(false);
        }
    }

    public void AddToInventory(ItemSlot itemslot)
    {
        if (!Inventory.IsFull() && itemslot.Item != null)
        {
            if (itemslot.Amount > 0)
                for (int i = 0; i < itemslot.Amount; i++)
                {
                    Inventory.AddItem(Instantiate(itemslot.Item.GetCopy()));
                }

            ActiveChest.itemProps.Remove(ActiveChest.itemProps.Find(item => item.item == itemslot.Item));
            itemslot.Item = null;

        }
    }
}