using UnityEngine;

namespace Script.QuestSystem
{
    public class InWorldQuestTarget: MonoBehaviour
    {
        public QuestTarget QuestTarget;
        public Quest quest;

        private void Start()
        {
            QuestTarget.TargetId = Random.Range(0, 100000);
        }
    }
}