﻿using System;
using Script.QuestSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.DialogSystem
{
    public class DialogUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI MessageTextHolder;
        [SerializeField] private Image AvatarPlaceHolder;
        [SerializeField] private Button ProgressButton;
        private int m_CurrentIndex, m_MaxIndex = 0;
        private DialogSender m_CurrentSender;


        public Action<DialogSender> OnCompeleteDialog;

        public DialogSender CurrentSender
        {
            get { return m_CurrentSender; }
            set
            {
                m_CurrentSender = value;
                if (m_CurrentSender == null)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(true);
                }
            }
        }


//        public static DialogUi Instance;
        private void Start()
        {
            ProgressButton.onClick.AddListener(DialogProgress);
            gameObject.SetActive(false);
        }

        public void GetData(DialogSender sender)
        {
            if (sender == null)
            {
                CurrentSender = null;
                return;
            }
            m_CurrentIndex = 0;
            CurrentSender = sender;
            m_MaxIndex = sender.dialog.messages.Count;
            DialogProgress();
        }

        private void DialogProgress()
        {
            if (m_CurrentIndex < m_MaxIndex && !m_CurrentSender.dialog.m_EndDialog)
            {
                MessageTextHolder.text = m_CurrentSender.dialog.messages[m_CurrentIndex].Content;
                m_CurrentIndex++;
            }
            else
            {
                m_CurrentSender.dialog.m_EndDialog = true;
                if (CurrentSender.Quest != null)
                {
                    //TODO Ghablan Inja Ham QuestInWorld Null CHeck Shode bood
                    QuestManager.Instance.QuestUi.SetData(CurrentSender.Quest);
                    
                    QuestManager.Instance.QuestUi.ToggleVisual(true);
                }
                CurrentSender = null;
            }
        }
    }
}

#region OldCode

/*
 *        [SerializeField] private GameObject dataCounter;
        [SerializeField] private Image AvatarHolder;
        [SerializeField] private TextMeshProUGUI messageHolder;
        [SerializeField] private Button dialogProgressButton;
        private Message m_CurrentMessage;
        private int m_Counter, m_CurrentIndex;
        private DialogSender m_DialogSender;

        public DialogSender DialogSender
        {
            get { return m_DialogSender; }
            set
            {
                m_DialogSender = value;
                
            }
        }

        void Start()
        {
            dataCounter.SetActive(false);
            dialogProgressButton.onClick.AddListener(OnDialogProgressButtonOnclick);
            m_DialogSender = null;
            EventManager.OnSendindDialog += OnSendindDialog;
            EventManager.OnCloseDialog += OnCloseDialog;
        }

        private void OnDialogProgressButtonOnclick()
        {
            if (m_CurrentIndex >= m_Counter)
            {
                dataCounter.SetActive(false);
                return;
            }

            DialogProgress();
        }


        private void OnSendindDialog(DialogSender dialogSender)
        {
            dataCounter.SetActive(true);
            if (dialogSender.Dialog.DialogCount() > 1)
                GameManager.Instance.GameContinue = false;
            m_Counter = dialogSender.Dialog.Messages.Count > dialogSender.Dialog.TargetMessage.Count
                ? dialogSender.Dialog.Messages.Count
                : dialogSender.Dialog.TargetMessage.Count;
            DialogSender = dialogSender;
            DialogProgress();
        }
        

        private void DialogProgress()
        {
            if (m_CurrentMessage == null)
            {
                m_CurrentIndex = 0;
                m_CurrentMessage = DialogSender.Dialog.Messages[m_CurrentIndex];
                AvatarHolder.sprite = m_DialogSender.Avatar;
            }
            else
            {
                if (m_CurrentMessage != DialogSender.Dialog.Messages[m_CurrentIndex] &&
                    DialogSender.Dialog.Messages.Count > m_CurrentIndex)
                {
                    m_CurrentMessage = DialogSender.Dialog.Messages[m_CurrentIndex];
                    AvatarHolder.sprite = DialogSender.Avatar;
                }
                else
                {
                    if (DialogSender.Dialog.TargetMessage.Count > m_CurrentIndex &&
                        DialogSender.Dialog.TargetMessage[m_CurrentIndex].massage != string.Empty)
                    {
                        AvatarHolder.sprite = DialogSender.Dialog.Target.GetComponent<SenderBase>().Avatar;
                        m_CurrentMessage = DialogSender.Dialog.TargetMessage[m_CurrentIndex];
                    }

                    m_CurrentIndex++;
                }
            }

            messageHolder.text = m_CurrentMessage.massage;

            if (m_CurrentIndex == m_Counter)
            {
                EventManager.OnCompeleteDialog?.Invoke(DialogSender);
                GameManager.Instance.GameContinue = true;
            }
        }

        private void OnCloseDialog()
        {
            GameManager.Instance.GameContinue = true;
            messageHolder.text = string.Empty;
            m_Counter = 0;
            m_CurrentIndex = 0;
            m_DialogSender = null;
            m_CurrentMessage = null;
            dataCounter.SetActive(false);
        }
 * 
 */

#endregion