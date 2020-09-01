using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

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
            Visitor.VisitorInteractableEvent += OnInteractableEvent;
            m_Inventory.AddItemEvent += InventoryOnAddItemEvent;
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
            foreach (var quest in QuestList)
            {
                if (quest.type == QuestType.Gathering)
                {
                    UpdateQuestItem(quest);
                }
            }
        }

        private void UpdateQuestItem(Quest quest)
        {
            QuestItem temp = quest.InWorldQuestTarget as QuestItem;
            if (temp != null)
            {
                quest.Amount = m_Inventory.ItemCount(temp.Item.Id);
                if (quest.Status == QuestStatus.Done)
                {
                    if (!m_Inventory.IsFull())
                        if (m_Inventory.EmptySlotCount() >= quest.Rewards.items.Count)
                        {
                            if (m_Inventory.RemoveItem(temp.Item, quest.MaxAmount))

                            {
                                foreach (var RewardItem in quest.Rewards.items)
                                {
                                    m_Inventory.AddItem(RewardItem.GetCopy());
                                }

                                m_QuestListUi.UpdateButton(quest);
                            }
                        }
                }
            }
        }


        private void QuestUiOnDismissEvent(Quest obj)
        {
            if (RemoveQuest(obj.questId))
            {
                obj.Status = QuestStatus.Waiting;
                m_QuestListUi.removeQuestButton(obj.questId);
                QuestUi.Quest = obj;
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
                UpdateQuestItem(obj);
            }
        }

        private void OnQuestButtonEvent(Quest quest)
        {
            QuestUi.SetData(quest);
        }

        public void OnInteractableEvent(QuestTarget quetTarget)
        {
            foreach (var quest in QuestList)
            {
                if (quest.InWorldQuestTarget.QuestTarget.TargetId == quetTarget.TargetId)
                {
                    quest.Amount++;
                    if (quest.Amount == quest.MaxAmount)
                    {
                        quest.Status = QuestStatus.Done;
                        m_QuestListUi.UpdateButton(quest);
                        if (quest.Status == QuestStatus.Done)
                        {
                            quest.GetRewards(InventoryManager.Instance.inventory);
                        }
                    }

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