using System;
using UnityEngine.UI;
using System.Collections.Generic;
using Script.QuestSystem;
using UnityEngine;
using Script.InventorySystem;
using Script.CurrencySystem;

namespace Script.ShopSystem
{
    public class ShopUi : MonoBehaviour
    {
        [HideInInspector] public bool Sell, Buy = false;
        [HideInInspector] public List<ShopItemSlot> m_SellItems;
        public Button clearDataButton;
        [SerializeField] private List<ShopItemSlot> shopItemSlots;
        [SerializeField] private ShopItemSlot shopItemSlot;
        [SerializeField] private List<TabButton> tabButtons;
        [SerializeField] private CanvasRenderer BuyTab, SellTab;
        [SerializeField] private UIAnimation uiAnimation;

        private void Awake()
        {
            clearDataButton.GetComponent<Button>().onClick.AddListener(ClearDataButtonOnclick);
            clearDataButton.interactable = Sell;
            foreach (var tabButton in tabButtons)
            {
                tabButton.buttonTypeOnclickEvent += TabButtonEvent;
            }
            SetData(null);
        }

        private void TabButtonEvent(ButtonType type)
        {
            //            Debug.Log("TabButton On ShopUi");
            if (type == ButtonType.Buy)
            {
                Buy = true;
                Sell = false;
                ShopManager.Instance.SellWindowActive = Sell;
                BuyTab.gameObject.SetActive(true);
                SellTab.gameObject.SetActive(false);
                clearDataButton.interactable = Sell;
            }

            if (type == ButtonType.Sell)
            {
                Buy = false;
                Sell = true;
                ShopManager.Instance.SellWindowActive = Sell;
                BuyTab.gameObject.SetActive(false);
                SellTab.gameObject.SetActive(true);
                if (m_SellItems.Count > 0)
                    clearDataButton.interactable = Sell;
            }
        }

        public bool SellTabSetData(ShopItem shopItem)
        {
            var temp = Instantiate(shopItemSlot, SellTab.transform);
            temp.SetData(shopItem);
            temp.PurchasingEvent += OnRebuySellItem;
            if (temp != null)
            {
                m_SellItems.Add(temp);
                clearDataButton.interactable = Sell;
                return true;
            }

            return false;
        }

        public void ClearDataButtonOnclick()
        {
            Debug.Log("destroy Itemslot");
            foreach (var itemSlot in m_SellItems)
            {
                Destroy(itemSlot.gameObject);
            }
            m_SellItems.Clear();
        }

        public void SetData(List<ShopItem> shopItems = null)
        {
            foreach (var itemSlot in shopItemSlots)
            {
                if (itemSlot != null)
                    Destroy(itemSlot.gameObject);
            }
            shopItemSlots.Clear();
            if (shopItems != null && shopItems.Count > 0)
            {
                    Debug.Log("ShopUi --> "+shopItems.Count);
                
                foreach (var item in shopItems)
                {
                    var x = Instantiate(shopItemSlot, BuyTab.transform);
                    x.SetData(item);
                    x.PurchasingEvent += OnPurchasingEvent;
                    shopItemSlots.Add(x);
                }
                uiAnimation.OpenPanel();
                uiAnimation.ActivateButtons(true);
            }
            else
            {
                uiAnimation.ActivateButtons(false);
                uiAnimation.ClosePanel();
                Sell = false;
                Buy = false;
                ShopManager.Instance.SellWindowActive = Sell;
            }
            TabButtonEvent(ButtonType.Buy);
        }

        private void OnPurchasingEvent(ShopItemSlot itemSlot)
        {
            if (CurrencyManager.Instance.CanPurchasing(itemSlot.ShopItem.item.BuyPrice * itemSlot.ShopItem.Amount))
            {
                if (!InventoryManager.Instance.inventory.IsFull())
                {
                    InventoryManager.Instance.inventory.AddItem
                        (
                        itemSlot.ShopItem.item.GetCopy(),
                        itemSlot.ShopItem.Amount,
                        itemSlot.ShopItem.Amount
                        );
                    CurrencyManager.Instance.AddGold
                        (
                        -itemSlot.ShopItem.item.BuyPrice * itemSlot.ShopItem.Amount
                        );
                    Destroy(itemSlot.gameObject);
                }
            }
        }

        private void OnRebuySellItem(ShopItemSlot itemSlot)
        {
            // ReSharper disable once PossibleLossOfFraction
            float g = itemSlot.ShopItem.item.BuyPrice * itemSlot.ShopItem.Amount * 30 / 100;
            if (g < 1)
                g = 1;
            if (CurrencyManager.Instance.CanPurchasing((int)g))
            {
                if (!InventoryManager.Instance.inventory.IsFull())
                {
                    InventoryManager.Instance.inventory.AddItem
                        (
                        itemSlot.ShopItem.item.GetCopy(),
                        itemSlot.ShopItem.Amount,
                        itemSlot.ShopItem.Amount
                        );

                    CurrencyManager.Instance.AddGold((int)-g);
                    if (itemSlot.ShopItem.Amount > 0)
                        return;
                    Destroy(itemSlot.gameObject);
                }
            }
        }
    }
}