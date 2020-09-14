using System.Collections.Generic;
using Script.QuestSystem;
using UnityEngine;

namespace Script.DialogSystem
{
    public class DialogSender : SenderBase
    {
        [SerializeField] public Dialog dialog;

        private void Start()
        {
            senderQuest.questId = Random.Range(0, 100000000);
        }

        [Space] [Header("-------------")] [SerializeField]
        public Quest senderQuest;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                DialogManager.Instance.dialogUi.GetData(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (senderQuest != null && senderQuest.InWorldQuestTarget != null)
                {
                    QuestManager.Instance.QuestUi.ToggleVisual(false);
                }
                DialogManager.Instance.dialogUi.GetData(null);
            }
        }
    }
}

#region OldCode

//[SerializeField] private int Id;
//public Dialog Dialog;
//public Transform dialogPlaceHolder;
//
//private void OnTriggerEnter2D(Collider2D other)
//{
//if(other.CompareTag("Player"))
//{
//Dialog.Target = other.transform;
//SendDialog();
//}
//}
//
//private void OnTriggerExit2D(Collider2D other)
//{
//if (other.CompareTag("Player"))
//{
//Dialog.Target = null;
//CloseDialog();
//}
//}
//
//private void Start()
//{
//Id = Random.Range(0, 100000);
//Dialog.SenderId = Id;
//}
//
//public void SendDialog()
//{
//EventManager.OnSendindDialog?.Invoke(this);
//}
//
//public void CloseDialog()
//{
//EventManager.OnCloseDialog?.Invoke();
//}

#endregion