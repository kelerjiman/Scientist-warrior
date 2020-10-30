using System;
using System.Collections;
using System.Collections.Generic;
using Script.CharacterStatus;
using Script.InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentPanel : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform EquipmentSlotParent;
    [SerializeField] private EquipmentSlot[] EquipmentSlots;
    public event Action<EquipableItem> EquipmentStateEvent;
    public event Action<ItemSlot> OnRightClickEvent;
    public event Action<ItemSlot> OnBeginDragEvent;
    public event Action<ItemSlot> OnDragEvent;
    public event Action<ItemSlot> OnEndDragEvent;
    public event Action<ItemSlot> OnDropEvent;
    public event Action<EquipableItem> OnAddItemEvent;
    public event Action<EquipableItem> OnRemoveItemEvent;
    public event Action OnDropVoidEvent;
    private void Start()
    {
        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            EquipmentSlots[i].OnRightClickEvent += OnRightClickEvent;
            EquipmentSlots[i].OnBeginDragEvent += OnBeginDragEvent;
            EquipmentSlots[i].OnDragEvent += OnDragEvent;
            EquipmentSlots[i].OnEndDragEvent += OnEndDragEvent;
            EquipmentSlots[i].OnDropEvent += OnDropEvent;
        }
    }

    private void OnValidate()
    {
        EquipmentSlots = EquipmentSlotParent.GetComponentsInChildren<EquipmentSlot>();
    }

    public bool AddItem(ItemSlot item, out EquipableItem PreviousItem, out int amount)
    {
        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            if (EquipmentSlots[i].CanRecieveItem(item.Item))
            {
                PreviousItem = (EquipableItem)EquipmentSlots[i].Item;
                amount = EquipmentSlots[i].Amount;
                EquipmentSlots[i].Item = item.Item;
                EquipmentSlots[i].Amount = item.Amount;
                OnAddItemEvent?.Invoke((EquipableItem)EquipmentSlots[i].Item);
                return true;
            }
            //if (EquipmentSlots[i].equipmentType == (item.Item as EquipableItem)?.EquipmentType)
            //{
            //    PreviousItem = (EquipableItem) EquipmentSlots[i].Item;
            //    amount = EquipmentSlots[i].Amount;
            //    EquipmentSlots[i].Item = item.Item;
            //    EquipmentSlots[i].Amount = item.Amount;
            //    return true;
            //}
        }

        PreviousItem = null;
        amount = 0;
        return false;
    }

    public bool RemoveItem(EquipableItem item)
    {
        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            if (EquipmentSlots[i].Item == item)
            {
//                CharacterScript.Instance.characterState.Remove(item.state);
                PlayerVisualScript.Instance.RemoveItemVisual(item);
                OnRemoveItemEvent?.Invoke(item);
                EquipmentSlots[i].Item = null;
                return true;
            }
        }

        return false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDropVoidEvent?.Invoke();
    }
}