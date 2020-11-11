using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    [Serializable]
    public struct ItemAmount
    {
        public Item item;
        [Range(1, 999)] public int itemAmount;
    }

    [CreateAssetMenu]
    public class CraftingRecipe : ScriptableObject
    {
        public List<ItemAmount> materials;
        public ItemAmount resault;

        public bool CanCraft(IItemContainer itemContainer)
        {
            foreach (var itemAmount in materials)
            {
                if (itemContainer.ContainItem(itemAmount.item))
                    Debug.Log(itemAmount.item.name);
                if (itemContainer.ItemCount(itemAmount.item.Id) < itemAmount.itemAmount)
                    return false;
            }

            return true;
        }

        public void Craft(IItemContainer itemContainer)
        {
            if (CanCraft(itemContainer))
            {
                foreach (var itemAmount in materials)
                {
                    for (int i = 0; i < itemAmount.itemAmount; i++)
                    {
                        /*Item oldItem =*/
                        itemContainer.RemoveItem(itemAmount.item.Id);
                        //                        if (oldItem != null)
                        //                            oldItem.Destroy();
                    }
                }

                itemContainer.AddItem(resault.item.GetCopy(), resault.itemAmount);
                (itemContainer as Inventory)?.AddItemEvent?.Invoke(resault.item);
            }
        }
    }
}