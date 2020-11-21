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
                    // ToggleVisual(true);
                    if (Quest.type == QuestType.Visiting)
                        m_Quest.Visitor.quest = m_Quest;
                    //TODO Quest Visitor ra be accept Button bebar
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

                        if (m_Quest.Status == QuestStatus.Done || m_Quest.Status == QuestStatus.Compelete)
                        {
                            TrackingButton.interactable = false;
                            DismissButton.interactable = false;
                        }

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
        public bool condition = false;

        private void Awake()
        {
            DismissButton.onClick.AddListener(DismissButtonOnclick);
        }

        public void SetData(Quest quest)
        {
            if(!condition)
                ToggleVisual(true);
            Quest = quest;
        }

        public void ToggleVisual(bool con)
        {
            var temp = visual.GetComponent<UIAnimation>();
            if (temp == null) return;
            if(con)
                temp.OpenPanel();
            else
                temp.ClosePanel();
            // visual.SetActive(con);
            condition = con;
            Debug.Log("ToggleVisual --> con "+con);
        }

        private void TrackingButtonOnclick()
        {
            TrackingEvent?.Invoke(Quest);
            ToggleVisual(false);
        }

        private void AcceptButtonOnclick()
        {
            AcceptEvent?.Invoke(Quest);
            if (Quest.Status != QuestStatus.NotAccepted)
                ToggleVisual(false);
                // visual.SetActive(false);
        }

        private void DismissButtonOnclick()
        {
            DismissEvent?.Invoke(Quest);
            ToggleVisual(false);
        }
    }
}