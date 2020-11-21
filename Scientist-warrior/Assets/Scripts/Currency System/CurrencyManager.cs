using System;
using UnityEngine;
using Script.QuestSystem;
using TMPro;
namespace Script.CurrencySystem
{
    
    public class CurrencyManager : MonoBehaviour
    {
        //bayad negah darande text coin va xp ra moshakhas kon
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private SliderScript xpSlider;
        [SerializeField] private TextMeshProUGUI GoldText;
        public event Action<string> SendLevelEvent; 
        private int _coin = 0; 
        public int Coin
        {
            get { return _coin; }
            set
            {
                _coin = value;
                GoldText.text = _coin.ToString();
            }
            
        }
        // private int gold = 0; 
        // public int Gold
        // {
        //     get { return gold; }
        // }
        // private int silver = 0; 
        // public int Silver
        // {
        //     get { return silver; }
        // }
        // private int copper = 0; 
        // public int Copper
        // {
        //     get { return copper; }
        // }

        private int gem = 0; 
        public int Gem
        {
            get { return gem; }
        }

        private int _experience = 1; 
        public int Experience
        {
            get { return _experience; }
            set
            {
                _experience = value;
                xpSlider.CurrentAmount = _experience;
            }
        }

        [SerializeField] private int level = 1; 
        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                levelText.text = value.ToString();
                SendLevelEvent?.Invoke(Level.ToString());
                
            }
        }

        [SerializeField] private int _nextLevelXp = 10; 
        public int NextLevelXp
        {
            get { return _nextLevelXp; }
            set
            {
                _nextLevelXp = value;
                xpSlider.MaxAmount = _nextLevelXp;
            }
        }

        public static CurrencyManager Instance;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            Npc.IncomeEvent += OnNpcIncomeEvent;
        }

        private void Start()
        {
            levelText.text = level.ToString();
            GoldText.text = Coin.ToString();
            xpSlider.CurrentAmount= Experience;
            xpSlider.MaxAmount = NextLevelXp;
        }

        public void OnNpcIncomeEvent(Income income)
        {
            Coin += income.GetCoin();
            AddXp(income.GetXp());
            AddGold(Coin);
        }

        public void AddGold(int coin)
        {
            // int coinTemp = coin;
            //  gold+= coinTemp/10000;
            // coinTemp -= gold * 10000;
            // silver = coinTemp / 100;
            // coinTemp -= silver * 100;
            // copper = coinTemp;
        }

        public void AddGem(int gem)
        {
            this.gem += gem;
        }

        public void AddXp(int xp)
        {
            Experience += xp;
            CalculateLevel();
        }

        private void CalculateLevel()
        {
            if (Experience >= _nextLevelXp)
            {
                Level += 1;
                NextLevelXp +=  NextLevelXp *2;
                CalculateLevel();
            }
        }

        public bool CanPurchasing(int amount)
        {
            if (Coin - amount >= 0)
                return true;
            return false;
        }
    }
}