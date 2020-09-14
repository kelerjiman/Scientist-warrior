using UnityEngine;

namespace Script.ShopSystem
{
    public class CurrencyManager : MonoBehaviour
    {
        [SerializeField] private int gold = 0;

        public int Gold
        {
            get { return gold; }
            set { gold = value; }
        }

        [SerializeField] private int gem = 0;

        public int Gem
        {
            get { return gem; }
            set { gem = value; }
        }

        [SerializeField] private int experience = 1;

        public int Experience
        {
            get { return experience; }
            set { experience = value; }
        }

        [SerializeField] private int level = 1;

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        [SerializeField] private int nextLevelXp = 10;

        public int NextLevelXp
        {
            get { return nextLevelXp; }
            set { nextLevelXp = value; }
        }

        public static CurrencyManager Instance;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void AddGold(int gold)
        {
            Gold += gold;
        }

        public void AddGem(int gem)
        {
            Gem += gem;
        }

        public void AddXp(int xp)
        {
            Experience += xp;
            CalculateLevel();
        }

        private void CalculateLevel()
        {
            if (Experience >= nextLevelXp)
            {
                Level += 1;
                nextLevelXp += Experience * Level / NextLevelXp + NextLevelXp;
                CalculateLevel();
            }
        }

        public bool CanPurchasing(int amount)
        {
            if (gold - amount >= 0)
                return true;
            return false;
        }
    }
}