using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class BaseCraftingWindow : MonoBehaviour
    {
        [SerializeField] private CraftingRecipeUi craftingUiPrefab; // prefab of crafting ui
        [SerializeField] private List<CraftingRecipe> craftingRecipes;
        [SerializeField] private RectTransform craftingUiParent; //place holder of crafting uis

        private List<CraftingRecipeUi>
            m_CraftingUIs = new List<CraftingRecipeUi>(); // count s depends on Crafting Recipes count

        public event Action<CraftingRecipe> CraftingRecipeHandler;

        private void Awake()
        {
            foreach (var recipe in craftingRecipes)
            {
                var newCraftingUi = Instantiate(craftingUiPrefab, craftingUiParent);
                newCraftingUi.CraftRecipe = recipe;
                m_CraftingUIs.Add(newCraftingUi);
            }

            foreach (var craftingUi in m_CraftingUIs)
            {
                craftingUi.CraftingRecipeHandler -= CraftingRecipeHandler;
                craftingUi.CraftingRecipeHandler += CraftingRecipeHandler;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}