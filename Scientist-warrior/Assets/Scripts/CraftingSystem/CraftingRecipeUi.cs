using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class CraftingRecipeUi : MonoBehaviour
    {
        public event Action<CraftingRecipe> CraftingRecipeHandler;
        private CraftingRecipe m_CraftRecipe;

        public CraftingItemSlot ItemSlotPrefab;

        public RectTransform MaterialsHolder;

        public RectTransform ResualtHolder;

        //        public CraftingItemSlot itemSlot1;
//        public CraftingItemSlot itemSlot2;
        public CraftingRecipe CraftRecipe
        {
            get { return m_CraftRecipe; }
            set
            {
                m_CraftRecipe = value;
                if (m_CraftRecipe != null)
                {
                    for (int i = 0; i < m_CraftRecipe.materials.Count; i++)
                    {
                        var x = Instantiate(ItemSlotPrefab, MaterialsHolder);
                        x.Item = m_CraftRecipe.materials[i].item;
                        x.Amount = m_CraftRecipe.materials[i].itemAmount;
                    }
                    resualt.Item = m_CraftRecipe.resault.item;
                    resualt.Amount = m_CraftRecipe.resault.itemAmount;
                }
//                itemSlot1.Item = m_CraftRecipe.materials[0].item;
//                itemSlot1.Amount = m_CraftRecipe.materials[0].itemAmount;
//                itemSlot2.Item = m_CraftRecipe.materials[1].item;
//                itemSlot2.Amount = m_CraftRecipe.materials[1].itemAmount;

//                resualt.Item = m_CraftRecipe.resault.item;
//                resualt.Amount = m_CraftRecipe.resault.itemAmount;
            }
        }

        public CraftingItemSlot resualt;

        [SerializeField] private Button craftButton;

        private void Start()
        {
            craftButton.onClick.RemoveAllListeners();
            craftButton.onClick.AddListener(OnCraftButtonOnclick);
        }

        public void OnCraftButtonOnclick()
        {
            CraftingRecipeHandler?.Invoke(CraftRecipe);
        }
    }
}