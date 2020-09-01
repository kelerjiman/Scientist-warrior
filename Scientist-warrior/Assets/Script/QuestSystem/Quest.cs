using System;
using UnityEngine;
using System.Collections.Generic;

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
        public int questId;
        public String name;
        [TextArea] public string description;
        public int order = -1;
        public List<Node> dependencies;
        public QuestStatus Status = QuestStatus.NotAccepted;
        [Space] public QuestRewards Rewards;
        [Space] public QuestType type;
        public InWorldQuestTarget InWorldQuestTarget;
        [SerializeField]private int amount = 0;

        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                if (amount >= MaxAmount)
                    Status = QuestStatus.Done;
            }
        }
        public int MaxAmount = 5;

        public void GetRewards(IItemContainer inventory)
        {
            foreach (var item in Rewards.items)
            {
                inventory.AddItem(item);
            }
        }
    }

    public enum QuestStatus
    {
        NotAccepted,
        Waiting,
        Current,
        Done
    }

    public enum QuestType
    {
        Killing,
        Gathering,
        Visiting,
        Puzzle
    }
}