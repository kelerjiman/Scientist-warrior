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
        public List<Message> Messages ;
        public List<Message> TargetMessage;
    }

    public enum DialogType
    {
        None
    }

    [Serializable]
    public struct Message
    {
        public string Title;

        [TextArea] public string massage;

    }
}
