using System;
using System.Collections.Generic;
using UnityEngine;


namespace Script.DialogSystem
{
    [Serializable]
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
}