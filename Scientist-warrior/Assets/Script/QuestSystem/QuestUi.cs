using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.QuestSystem
{
    public class QuestUi : MonoBehaviour
    {
        public event Action<Quest> AcceptEvent;
        public event Action<Quest> TrackingEvent;
        public event Action<Quest> DismissEvent;
        [SerializeField] private Quest m_Quest;

        public Quest Quest
        {
            get { return m_Quest; }
            set
            {
                m_Quest = value;
                if (m_Quest != null)
                {
                    TrackingButton.interactable = true;
                    
                    DismissButton.interactable = true;
                    for (int i = 0; i < RewardItemIconHParent.childCount; i++)
                    {
                        Destroy(RewardItemIconHParent.GetChild(i).gameObject);
                    }
                    foreach (var item in m_Quest.Rewards.items)
                    {
                        Icon.sprite = item.Icon;
                        Instantiate(Icon, RewardItemIconHParent);
                    }
                    if (!QuestManager.Instance.QuestWasAdded(m_Quest.questId))
                    {
                        // set button to accept and after press add quest to quest manager
                        TrackingButton.GetComponentInChildren<Text>().text = "Accept";
                        TrackingButton.onClick.RemoveAllListeners();
                        TrackingButton.onClick.AddListener(AcceptButtonOnclick);
                    }
                    else
                    {
                        // set button to Active and after press track quest
                        if (m_Quest.Status == QuestStatus.Waiting)
                        {
                            TrackingButton.GetComponentInChildren<Text>().text = "Active";
                            TrackingButton.onClick.RemoveAllListeners();
                            TrackingButton.onClick.AddListener(TrackingButtonOnclick);
                        }

                        if (m_Quest.Status == QuestStatus.Current)
                        {
                            TrackingButton.GetComponentInChildren<Text>().text = "Actived";
                            TrackingButton.onClick.RemoveAllListeners();
                            TrackingButton.onClick.AddListener(TrackingButtonOnclick);
                        }

                        if (m_Quest.Status == QuestStatus.Done)
                        {
                            TrackingButton.interactable = false;
                            DismissButton.interactable = false;
                        }
                        ToggleVisual(true);
                    }

                    QuestTitle.text = m_Quest.name;
                    QuestDescription.text = m_Quest.description;
                }
                else
                {
                    QuestTitle.text = string.Empty;
                    QuestDescription.text = string.Empty;
                    ToggleVisual(false);
                }
            }
        }

        [SerializeField] private GameObject visual;
        [SerializeField] private Text QuestTitle, QuestDescription;
        [SerializeField] private RectTransform QuestItemIconParent, RewardItemIconHParent;
        [SerializeField] private Image Icon;
        [Space] [SerializeField] private Button DismissButton, TrackingButton;


        private void Awake()
        {
            DismissButton.onClick.AddListener(DismissButtonOnclick);
        }

        public void SetData(Quest quest)
        {
            Quest = quest;
        }

        public void ToggleVisual(bool con)
        {
            visual.SetActive(con);
        }

        private void TrackingButtonOnclick()
        {
            TrackingEvent?.Invoke(Quest);
        }

        private void AcceptButtonOnclick()
        {
            AcceptEvent?.Invoke(Quest);
            if (Quest.Status != QuestStatus.NotAccepted)
                visual.SetActive(false);
        }

        private void DismissButtonOnclick()
        {
            DismissEvent?.Invoke(Quest);
        }
    }
}