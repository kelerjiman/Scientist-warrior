using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Script.DialogSystem;
using Script.QuestSystem;
using UnityEngine;

public static class EventManager
{
    public static Action<DialogSender> OnSendindDialog;
    public static Action<DialogSender> OnCompeleteDialog;
    public static Action OnCloseDialog;
}
