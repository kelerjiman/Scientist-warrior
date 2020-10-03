using System;
using System.Collections.Generic;
using Script.InventorySystem;
using UnityEngine;

namespace Script.ShopSystem
{
    public class ShopManager : MonoBehaviour
    {
        public bool SellWindowActive { get; set; }

        public ShopUi m_ShopUi;
        public static ShopManager Instance;
        public event Action<List<ShopItem>> SellItemEvent;

        private void Awake()
        {
            InventoryManager.Instance.inventory.OnRightClickEvent += InventoryOnOnRightClickEvent;
            Instance = this;
        }

        private void InventoryOnOnRightClickEvent(ItemSlot itemSlot)
        {
            Debug.Log("shop manager");
            if (m_ShopUi.Buy)
                return;
            ShopItem newShopItem = new ShopItem
            {
                item = itemSlot.Item,
                Amount = itemSlot.Amount
            };
//            m_ShopUi.m_SellItems.Add(newShopItem);
            // ReSharper disable once PossibleLossOfFraction
            float g = itemSlot.Amount * itemSlot.Item.BuyPrice * 30 / 100;
            if (g < 1)
                g = 1;
            if (m_ShopUi.SellTabSetData(newShopItem))
            {
                CurrencyManager.Instance.AddGold((int) g);
                InventoryManager.Instance.inventory.RemoveItem(itemSlot.Item);
                itemSlot.Item = null;
            }
        }

        private void Start()
        {
            m_ShopUi.gameObject.SetActive(false);
            Instance = this;
        }
    }
}