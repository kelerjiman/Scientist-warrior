using System;
using UnityEngine;

namespace Script.ShopSystem
{
    [Serializable]
    public class ShopItem
    {
        public Item item;
        [TextArea] public string summary;
        public int Amount = 1;
        
    }
}