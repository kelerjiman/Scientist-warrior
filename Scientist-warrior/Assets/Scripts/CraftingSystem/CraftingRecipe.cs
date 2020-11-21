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

        public bool CanCraft(IItemContainer inventory)
        {
            foreach (var itemAmount in materials)
            {
                if (!inventory.ContainItem(itemAmount.item))
                    return false;
                    Debug.Log(itemAmount.item.name);
                if (inventory.ItemCount(itemAmount.item.Id) < itemAmount.itemAmount)
                    return false;
            }

            return true;
        }

        public void Craft(IItemContainer inventory)
        {
            if (CanCraft(inventory))
            {
                foreach (var itemAmount in materials)
                {
                    /*Item oldItem =*/
                    inventory.RemoveItem(itemAmount.item.Id, itemAmount.itemAmount);
                    //                        if (oldItem != null)
                    //                            oldItem.Destroy();
                }

                int t;
                inventory.AddItem(resault.item.GetCopy(), resault.itemAmount,out t);
                (inventory as Inventory)?.AddItemEvent?.Invoke(resault.item);
            }
        }
    }
}