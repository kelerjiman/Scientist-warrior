using System;
using Script.DialogSystem;
using UnityEngine;

namespace Script.QuestSystem
{
    public class Visitor : InWorldQuestTarget
    {
        public static event Action<QuestTarget> VisitorInteractableEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && quest.Status!= QuestStatus.Compelete && quest.Status!= QuestStatus.Done)
                Condition();
        }

        void Condition()
        {
            VisitorInteractableEvent?.Invoke(QuestTarget);
        }
    }
}