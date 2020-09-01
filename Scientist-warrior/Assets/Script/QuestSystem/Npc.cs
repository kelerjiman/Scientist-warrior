using System;

namespace Script.QuestSystem
{
    public class Npc : InWorldQuestTarget
    {
        public static event Action<QuestTarget> NpcInteractableEvent;


        private void OnDestroy()
        {
            NpcInteractableEvent?.Invoke(QuestTarget);
        }

        public QuestTarget GetQuestTarget()
        {
            return QuestTarget;
        }
    }
}