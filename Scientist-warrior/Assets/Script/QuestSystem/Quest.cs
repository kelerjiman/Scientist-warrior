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
        public InWorldQuestTarget InWorldQuestTarget;
        [SerializeField] private int amount = 0;

        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                if (amount >= MaxAmount)
                {
                    Status = QuestStatus.Done;
                    if (GetRewards(InventoryManager.Instance.inventory))
                    {
                        if (type == QuestType.Gathering)
                        {
                            for (int i = 0; i < amount; i++)
                            {
                                InventoryManager.Instance.inventory.RemoveItem(((QuestItem)InWorldQuestTarget).Item);
                            }
                        }
                        Status = QuestStatus.Compelete;
                    }
                }
            }
        }

        public int MaxAmount = 5;

        public bool GetRewards(IItemContainer inventory)
        {
            foreach (var item in Rewards.items)
            {
                Debug.Log(item.ItemName);
                inventory.AddItem(item);
                //                (inventory as Inventory)?.AddItemEvent?.Invoke(item);
            }

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