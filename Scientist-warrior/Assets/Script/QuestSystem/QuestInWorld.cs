using System;
using Script.DialogSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.QuestSystem
{
    public class QuestInWorld : MonoBehaviour
    {
        [SerializeField] private Quest quest;

        private void Start()
        {
            quest.questId = Random.Range(0, 1000000);
        }

//       

        public Quest Quest
        {
            get { return quest; }
            set
            {
                quest = value;
                if (quest == null)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (quest == null)
                    return;
                QuestManager.Instance.QuestUi.SetData(Quest);
                QuestManager.Instance.QuestUi.ToggleVisual(true);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (quest == null)
                return;
            if (Quest.Status != QuestStatus.NotAccepted)
            {
                Quest = null;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                QuestManager.Instance.QuestUi.SetData(null);
                QuestManager.Instance.QuestUi.ToggleVisual(false);
            }
        }
    }
}