using System;
using System.Collections;
using System.Collections.Generic;
using Script.DialogSystem;
using UnityEngine;

public static class EventManager
{
    public static Action<ItemSlot> EquipItem;
    public static Action<ItemSlot> UnEquipItem;

    public static Action<DialogSender> OnSendindDialog;
    public static Action OnCloseDialog;
}
