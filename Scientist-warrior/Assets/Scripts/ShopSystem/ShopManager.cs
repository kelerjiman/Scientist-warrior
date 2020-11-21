using System;
using System.Collections.Generic;
using Script.InventorySystem;
using Script.CurrencySystem;
using UnityEngine;

namespace Script.ShopSystem
{
    public class ShopManager : MonoBehaviour
    {
        public bool SellWindowActive { get; set; }

        public ShopUi m_ShopUi;
        public static ShopManager Instance;
        public event Action<List<ShopItem>> SellItemEvent;
        public event Action<List<ShopItem>> ShopUiSetData;

        private void Awake()
        {
            InventoryManager.Instance.inventory.OnRightClickEvent += InventoryOnOnRightClickEvent;
            InWorldShop.ShopUiSetDataEvent+= ShopManager_ShopUiSetData;
            Instance = this;
        }

        private void InventoryOnOnRightClickEvent(ItemSlot itemSlot)
        {

            if (m_ShopUi.Buy || !m_ShopUi.gameObject.activeSelf)
                return;
            ShopItem newShopItem = new ShopItem
            {
                item = itemSlot.Item,
                Amount = itemSlot.Amount
            };
            //            m_ShopUi.m_SellItems.Add(newShopItem);
            // ReSharper disable once PossibleLossOfFraction
            //g meghdar gold ro hesab mikone bayad vaghti ke tedad set shod hesab beshe
            float g = itemSlot.Amount * itemSlot.Item.BuyPrice * 30 / 100;
            if (g < 1)
                g = 1;
            if (m_ShopUi.SellTabSetData(newShopItem))
            {
                CurrencyManager.Instance.AddGold((int)g);
                //itemSlot.amount tedade frosh hast
                InventoryManager.Instance.inventory.RemoveItem(itemSlot.Item, itemSlot.Amount);
                itemSlot.Item = null;
            }
        }

        private void Start()
        {
            Instance = this;
        }

        private void ShopManager_ShopUiSetData(List<ShopItem> ShopItems)
        {
            Debug.Log("ShopManager_ShopUiSetData");
            Debug.Log(ShopItems.Count);
            m_ShopUi.SetData(ShopItems);
        }
    }
}