using System;
using UnityEngine;

namespace Script.QuestSystem
{
    public class Visitor : InWorldQuestTarget
    {
        public static event Action<QuestTarget> VisitorInteractableEvent;


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                Condition();
        }

        void Condition()
        {
            VisitorInteractableEvent?.Invoke(QuestTarget);
        }

        public QuestTarget GetQuestTarget()
        {
            return QuestTarget;
        }
    }
}
//todo item slot Moonde