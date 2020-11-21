using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Script.InventorySystem;

namespace Script.QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] public List<Quest> QuestList;
        [Space] public QuestUi QuestUi;
        public QuestListUI QuestListUi;
        public static QuestManager Instance;
        private Inventory m_Inventory;

        private void Awake()
        {
            m_Inventory = InventoryManager.Instance.inventory;
            QuestUi.AcceptEvent += QuestUiOnAcceptEvent;
            QuestUi.TrackingEvent += QuestUiOnTrackingEvent;
            QuestUi.DismissEvent += QuestUiOnDismissEvent;
            QuestListUi.OnQuestButtonEvent += OnQuestButtonEvent;
            //Npc.NpcInteractableEvent += Npc_NpcInteractableEvent;
            Npc.NpcInteractableEvent += Npc_NpcInteractableEvent;
            Visitor.VisitorInteractableEvent += VisitorOnVisitorInteractableEvent;
            m_Inventory.AddItemEvent += InventoryOnAddItemEvent;
        }

        private void Start() => Instance = this;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (QuestListUi.gameObject.activeSelf)
                    QuestUi.ToggleVisual(false);
                QuestListUi.gameObject.SetActive(!QuestListUi.gameObject.activeSelf);
            }
        }

        public void AddQuest(Quest quest)
        {
            QuestList.Add(quest);
            QuestListUi.AddQuestButton(quest);
        }

        public bool QuestWasAdded(int questId)
        {
            foreach (var q in QuestList)
            {
                if (questId == q.questId)
                    return true;
            }

            return false;
        }

        public void RemoveQuest(int QuestId)
        {
            foreach (var q in QuestList)
            {
                if (q.questId == QuestId)
                {
                    QuestList.Remove(q);
                    return;
                }
            }
        }

        private Quest UpdateQuest(int questId)
        {
            var quest = QuestList.Find(q => q.questId == questId);
            if (quest == null)
                return null;
            quest.Amount++;
            return quest;
        }

        private Quest UpdateQuest(Item item)
        {
            Quest quest;
            quest = QuestList.Find(q =>
                q.type == QuestType.Gathering
                && q.QuestItem != null
                && q.QuestItem.Id == item.Id
            );
            if (quest != null)
                quest.Amount++;
            return quest;
        }

        private void UpdateQuest(Item item, Quest quest)
        {
            if (quest != null) quest.Amount = m_Inventory.ItemCount(item.Id);
        }

        public void CompeleteQuest(Quest quest)
        {
            if (quest == null)
                return;
            //if (quest.Status == QuestStatus.Done)
            //    return;
            if (quest.Status == QuestStatus.Compelete)
            {
                quest.questId *= -1;
                //TODO bayad GetReward ro fekr koni roosh
                quest.GetRewards(m_Inventory);
                Debug.Log("Compelete quest after get reward");
                quest.Status = QuestStatus.Done;
            }

            QuestListUi.UpdateButton(quest);
        }

        private void Npc_NpcInteractableEvent(int questId)
        {
            var quest = UpdateQuest(questId);
            CompeleteQuest(quest);
            SpawnerManager.Instance.SetQuest(quest);
        }

        private void VisitorOnVisitorInteractableEvent(int questId)
        {
            var quest = UpdateQuest(questId);
            CompeleteQuest(quest);
            //TODO Visitor Event ra Bayad Bazbini koni
        }

        private void InventoryOnAddItemEvent(Item item)
        {
            var quest = UpdateQuest(item);
            CompeleteQuest(quest);
            SpawnerManager.Instance.SetQuest(quest);
        }

        private void QuestUiOnDismissEvent(Quest quest)
        {
            if (quest.Status == QuestStatus.Current)
                quest.Status = QuestStatus.Waiting;
            else
            {
                RemoveQuest(quest.questId);
                QuestListUi.RemoveQuestButton(quest.questId);
                QuestUi.SetData(quest);
            }
        }

        private void QuestUiOnTrackingEvent(Quest obj)
        {
            if (obj.Status != QuestStatus.Done)
            {
                obj.Status = obj.Status != QuestStatus.Current ? QuestStatus.Current : QuestStatus.Waiting;
                QuestUi.Quest = obj;
            }

            QuestListUi.UpdateButton(obj);
        }

        private void QuestUiOnAcceptEvent(Quest quest)
        {
            if (quest != null)
            {
                quest.Status = QuestStatus.Waiting;
                AddQuest(quest);
                var itemQuest = quest.QuestItem;
                if (itemQuest != null)
                {
                    UpdateQuest(itemQuest, quest);
                    CompeleteQuest(quest);
                }

                if (quest.Npc != null)
                    SpawnerManager.Instance.SetQuest(quest);
                else if (quest.Visitor != null)
                    Debug.Log("visitor");
            }

            //TODO visitor bayad dorost shavad
        }

        private void OnQuestButtonEvent(Quest quest)
        {
            if (QuestUi.Quest.questId == quest.questId && QuestUi.condition)
            {
                QuestUi.ToggleVisual(!QuestUi.condition);
            }
            else
            {
                QuestUi.SetData(quest);
            }
        }
    }
}