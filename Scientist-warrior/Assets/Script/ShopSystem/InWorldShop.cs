using System.Collections.Generic;
using UnityEngine;

namespace Script.ShopSystem
{
    public class InWorldShop : MonoBehaviour
    {
        [SerializeField] private List<ShopItem> m_ShopItems;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ShopManager.Instance.m_ShopUi.SetData(m_ShopItems, this);
                ShopManager.Instance.m_ShopUi.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ShopManager.Instance.m_ShopUi.gameObject.SetActive(false);
            }
        }
    }
}