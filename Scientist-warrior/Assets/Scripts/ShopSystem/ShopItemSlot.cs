using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.ShopSystem
{
    public class ShopItemSlot : MonoBehaviour
    {
        private ShopItem m_ShopItem;

        public ShopItem ShopItem
        {
            get { return m_ShopItem; }
            set { m_ShopItem = value; }
        }

        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI nameHolder, summaryHolder, priceHolder,AmountHolder;
        [SerializeField] private Button puchaseButton;
        public event Action<ShopItemSlot> PurchasingEvent;

        private void Start()
        {
            puchaseButton.onClick.AddListener(PurchaseItem);
        }

        public void SetData(ShopItem shopItem)
        {
            ShopItem = shopItem;
            icon.sprite = shopItem.item.Icon;
            nameHolder.text = shopItem.item.ItemName;
            summaryHolder.text = shopItem.summary;
            priceHolder.text = shopItem.item.BuyPrice.ToString();
            AmountHolder.text = shopItem.Amount.ToString();
        }

        void PurchaseItem()
        {
            PurchasingEvent?.Invoke(this);
        }
    }
}