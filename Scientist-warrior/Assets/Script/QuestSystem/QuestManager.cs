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
        [Space] [SerializeField] public QuestUi QuestUi;
        public QuestListUI m_QuestListUi;
        public static QuestManager Instance;
        private Inventory m_Inventory;

        private void Awake()
        {
            m_Inventory = InventoryManager.Instance.inventory;
            QuestUi.AcceptEvent += QuestUiOnAcceptEvent;
            QuestUi.TrackingEvent += QuestUiOnTrackingEvent;
            QuestUi.DismissEvent += QuestUiOnDismissEvent;
            m_QuestListUi.OnQuestButtonEvent += OnQuestButtonEvent;
            Npc.NpcInteractableEvent += OnInteractableEvent;
            Visitor.VisitorInteractableEvent += VisitorOnVisitorInteractableEvent;
            m_Inventory.AddItemEvent += InventoryOnAddItemEvent;
        }

        private void VisitorOnVisitorInteractableEvent(QuestTarget questTarget)
        {
            foreach (var quest in QuestList)
            {
                if (quest.InWorldQuestTarget.QuestTarget.TargetId == questTarget.TargetId)
                {
                    quest.Amount++;
                    m_QuestListUi.UpdateButton(quest);
                    break;
                }

//                if (quest.Status == QuestStatus.Compelete ||)
//                {
//                    m_QuestListUi.UpdateButton(quest);
//                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (m_QuestListUi.gameObject.activeSelf)
                    QuestUi.ToggleVisual(false);
                m_QuestListUi.gameObject.SetActive(!m_QuestListUi.gameObject.activeSelf);
            }
        }

        private void InventoryOnAddItemEvent(Item item)
        {
            UpdateQuestItem(item);
//            var TempQuest = QuestList.Find(quest => (quest.InWorldQuestTarget as QuestItem)?.Item == item);
//            if (TempQuest != null)
//            {
//                Debug.Log("QuestManager ----> InventoryAddItemEvent");
//                UpdateQuestItem(item, TempQuest);
//            }
//
//            foreach (var quest in QuestList)
//            {
//                if (quest.type == QuestType.Gathering)
//                {
//                    var itemQuest = quest.InWorldQuestTarget as QuestItem;
//                    if (itemQuest.Item == item)
//                    {
//                        Debug.Log("QuestManager ----> InventoryAddItemEvent");
//                        UpdateQuestItem(item, quest);
//                    }
//                }
//            }
        }

        private void UpdateQuestItem(Item item, Quest quest)
        {
            quest.Amount = m_Inventory.ItemCount(item.Id);
            m_QuestListUi.UpdateButton(quest);
        }

        private void UpdateQuestItem(Item item)
        {
            foreach (var q in QuestList)
            {
                if (q.type == QuestType.Gathering)
                    if (q.InWorldQuestTarget as QuestItem != null)
                    {
                        q.Amount = m_Inventory.ItemCount(item.Id);
                        m_QuestListUi.UpdateButton(q);
                    }
            }

            
        }


        private void QuestUiOnDismissEvent(Quest quest)
        {
            if(quest.Status==QuestStatus.Current)
                quest.Status = QuestStatus.Waiting;
            else
            {
                RemoveQuest(quest.questId);
                m_QuestListUi.RemoveQuestButton(quest.questId);
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

            m_QuestListUi.UpdateButton(obj);
        }

        private void QuestUiOnAcceptEvent(Quest obj)
        {
            obj.Status = QuestStatus.Waiting;
            AddQuest(obj);
            if (obj.type == QuestType.Gathering)
            {
                var itemQuest = obj.InWorldQuestTarget as QuestItem;
                if (itemQuest.Item != null)
                {
                    UpdateQuestItem(itemQuest.Item, obj);
                }
            }
        }

        private void OnQuestButtonEvent(Quest quest)
        {
            QuestUi.SetData(quest);
        }

        public void OnInteractableEvent(QuestTarget questTarget)
        {
            foreach (var quest in QuestList)
            {
                if (quest.InWorldQuestTarget.QuestTarget.TargetId == questTarget.TargetId)
                {
                    quest.Amount++;
                    m_QuestListUi.UpdateButton(quest);
                    break;
                }
            }
        }

        private void Start()
        {
            Instance = this;
        }


        public void AddQuest(Quest quest)
        {
            QuestList.Add(quest);
            m_QuestListUi.addQuestButton(quest);
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

        public bool RemoveQuest(int QuestId)
        {
            foreach (var q in QuestList)
            {
                if (q.questId == QuestId)
                {
                    QuestList.Remove(q);
                    return true;
                }
            }

            return false;
        }

        public void UpdateQuest(Quest quest)
        {
            var index = QuestList.FindIndex(q => q.questId == quest.questId);
            QuestList[index] = quest;
        }
    }
}