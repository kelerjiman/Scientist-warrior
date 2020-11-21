using System;

namespace Script.ShopSystem
{
    [Serializable]
    public class Price
    {
        public PriceType type;
        public int goldAmount = 1;
        public int gemAmount = 1;
    }

    public enum PriceType
    {
        Gold,
        Gem,
        Both
    }
}
