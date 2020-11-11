using UnityEngine;

namespace Script.QuestSystem
{
    public class InWorldQuestTarget : MonoBehaviour
    {
        [SerializeField] protected QuestTarget QuestTarget;
        //public Quest quest;
        public QuestTarget GetQuestTarget()
        {
            return QuestTarget;
        }

        //private void Start()
        //{
        //    QuestTarget.TargetId = Random.Range(0, 100000);
        //}
    }
}