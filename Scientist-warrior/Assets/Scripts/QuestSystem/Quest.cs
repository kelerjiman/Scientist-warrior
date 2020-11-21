using System;
using UnityEngine;
using System.Collections.Generic;
using Script.InventorySystem;

namespace Script.QuestSystem
{
    [Serializable]
    public struct Node
    {
        public Quest before;
        public Quest after;
    }

    [Serializable]
    public class Quest
    {
        public string name;
        public int questId;

        [TextArea] public string description;
        public int order = -1;
        public List<Node> dependencies;
        public QuestStatus Status = QuestStatus.NotAccepted;
        [Space] public QuestRewards Rewards;
        [Space] public QuestType type;
        public Item QuestItem;
        public Npc Npc;
        public Visitor Visitor;
        [SerializeField] private int amount = 0;

        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                if (amount >= MaxAmount)
                {
                    amount = MaxAmount;
                    if (questId < 0)
                        return;
                    Status = QuestStatus.Compelete;

                }
            }
        }

        public int MaxAmount = 5;

        public bool GetRewards(IItemContainer inventory)
        {
            //foreach (var item in Rewards.items)
            //{
            //    int t;
            //    Debug.Log("GetRewards " + item.ItemName);
            //    inventory.AddItem(item, 1,out t);
            //    //                (inventory as Inventory)?.AddItemEvent?.Invoke(item);
            //}
            inventory.AddItem(Rewards.items);
            if (type == QuestType.Gathering)
            {
                InventoryManager.Instance.inventory.RemoveItem(QuestItem, Amount);
            }
            Status = QuestStatus.Compelete;

            return true;
        }
    }

    public enum QuestStatus
    {
        NotAccepted,
        Waiting,
        Current,
        Done,
        Compelete
    }

    public enum QuestType
    {
        Killing,
        Gathering,
        Visiting,
        Puzzle
    }
}