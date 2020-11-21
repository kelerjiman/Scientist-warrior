using System.Collections.Generic;
using UnityEngine;

namespace Script.ShopSystem
{
    using System;
    public class InWorldShop : MonoBehaviour
    {
        [SerializeField] private List<ShopItem> m_ShopItems;
        public static event Action<List<ShopItem>> ShopUiSetDataEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                //ShopManager.Instance.m_ShopUi.SetData(m_ShopItems);
                ShopUiSetDataEvent?.Invoke(m_ShopItems);
                ShopManager.Instance.m_ShopUi.GetComponent<UIAnimation>().OpenPanel();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {

                ShopManager.Instance.m_ShopUi.SetData(null);
                ShopManager.Instance.m_ShopUi.GetComponent<UIAnimation>().ClosePanel();
            }
        }
    }
}