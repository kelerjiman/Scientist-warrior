using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class CraftingRecipeUi : MonoBehaviour
    {
        public event Action<CraftingRecipe> CraftingRecipeHandler;
        private CraftingRecipe m_CraftRecipe;

        public CraftingRecipe CraftRecipe
        {
            get { return m_CraftRecipe; }
            set
            {
                m_CraftRecipe = value;
                itemSlot1.Item = m_CraftRecipe.materials[0].item;
                itemSlot1.Amount = m_CraftRecipe.materials[0].itemAmount;
                itemSlot2.Item = m_CraftRecipe.materials[1].item;
                itemSlot2.Amount = m_CraftRecipe.materials[1].itemAmount;
                resualt.Item = m_CraftRecipe.resault.item;
                resualt.Amount = m_CraftRecipe.resault.itemAmount;
            }
        }

        public CraftingItemSlot itemSlot1;
        public CraftingItemSlot itemSlot2;
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