using System;
using System.Collections.Generic;

namespace Script.QuestSystem
{
    [Serializable]
    public class QuestRewards
    {
        public int XP = 1;
        public List<Item> items;
        public int Gold = 0;
        public int silver = 0;
        public int cooper = 0;
    }
}