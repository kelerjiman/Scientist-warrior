using System;
using System.Collections.Generic;
using UnityEngine;


namespace Script.DialogSystem
{
    [Serializable]
    public class Dialog
    {
        [SerializeField] public List<Message> messages;
        public bool m_EndDialog = false;
        private int m_Id;
    }
[Serializable]
    public struct Message
    {
        public Turn turn;
        [TextArea] public string Content;
    }

    public enum Turn
    {
        Sender,
        Opponent
    }
}

#region OldCode

/*
 *  [Serializable]
    public class Dialog
    {
        public string Name;
        public int Id;
        public int SenderId;
        public DialogType type;
        public List<Message> Messages;
        public List<Message> TargetMessage;
        [HideInInspector] public Transform Target;

        public int DialogCount()
        {
            return Messages.Count + TargetMessage.Count;
        }
    }

    public enum DialogType
    {
        Normal,
        Argue
    }

    [Serializable]
    public class Message
    {
        public string Title;

        [TextArea] public string massage;
    }
 */

#endregion