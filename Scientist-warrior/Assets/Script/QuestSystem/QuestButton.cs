using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.QuestSystem
{
    public class QuestButton : MonoBehaviour
    {
        [SerializeField] private Quest m_Quest;
        [SerializeField] private Image texture;
        public event Action<Quest> QuestButtonEvent;

        public Quest Quest
        {
            get { return m_Quest; }
            set
            {
                m_Quest = value;
                ChangeTexture();
            }
        }

        private void ChangeTexture()
        {
            if (Quest.Status == QuestStatus.Current)
            {
                texture.color = Color.green;
            }

            if (Quest.Status == QuestStatus.Waiting)
            {
                texture.color = Color.white;
            }

            if (Quest.Status == QuestStatus.Done)
            {
                texture.color = Color.blue;
            }
        }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClickButton);
        }

        private void LateUpdate()
        {
            if (m_Quest.Status == QuestStatus.Done)
            {
                Quest = m_Quest;
            }
        }

        private void OnClickButton()
        {
            QuestButtonEvent?.Invoke(Quest);
        }
    }
}
