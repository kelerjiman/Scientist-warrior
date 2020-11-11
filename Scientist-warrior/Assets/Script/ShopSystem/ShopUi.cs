using System;
using System.Collections.Generic;
using Script.InventorySystem;
using Script.QuestSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Script.ShopSystem
{
    public class ShopUi : MonoBehaviour
    {
        [HideInInspector] public bool Sell, Buy = false;
        [HideInInspector] public List<ShopItemSlot> m_SellItems;
        public Button clearDataButton;
        [SerializeField] private List<ShopItem> items;
        [SerializeField] private List<ShopItemSlot> shopItemSlots;

        [SerializeField] private ShopItemSlot shopItemSlot;

        //        [SerializeField] private RectTransform itemSlotPlaceHolder;
        [SerializeField] private List<TabButton> tabButtons;
        [SerializeField] private CanvasRenderer BuyTab, SellTab;
        private InventoryManager m_InventoryManager;
        private CurrencyManager m_CurrencyManager;

        private void Awake()
        {
            clearDataButton.GetComponent<Button>().onClick.AddListener(ClearDataButtonOnclick);
            clearDataButton.interactable = Sell;
            foreach (var tabButton in tabButtons)
            {
                tabButton.buttonTypeOnclickEvent += TabButtonEvent;
            }

            SetData(items);
            TabButtonEvent(ButtonType.Buy);
        }

        private void OnEnable()
        {
            TabButtonEvent(ButtonType.Buy);
        }

        private void OnDisable()
        {
            Sell = false;
            Buy = false;
            ShopManager.Instance.SellWindowActive = Sell;
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
            items = shopItems;
            foreach (var itemSlot in shopItemSlots)
            {
                if (itemSlot != null)
                    Destroy(itemSlot.gameObject);
            }
            shopItemSlots.Clear();
            if (items != null)
                foreach (var item in items)
                {
                    var x = Instantiate(shopItemSlot, BuyTab.transform);
                    x.SetData(item);
                    x.PurchasingEvent += OnPurchasingEvent;
                    shopItemSlots.Add(x);
                }
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
                        itemSlot.ShopItem.Amount
                        );

                    CurrencyManager.Instance.AddGold((int)-g);
                    Destroy(itemSlot.gameObject);
                }
            }
        }
    }
}