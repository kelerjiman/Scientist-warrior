using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script;
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

    //public bool AddItem(EquipableItem item, int amount, out List<Item> PreviousItems)
    //{
    //    PreviousItems = new List<Item>();
    //    var target = EquipmentSlots.FirstOrDefault(s => s.EquipmentType == item.EquipmentType);
    //    if (target != null)
    //    {
    //        PreviousItems.Add((EquipableItem)target.Item);
    //        RemoveItem(PreviousItems);
    //        target.Item = item;
    //        target.Amount = amount;
    //        OnAddItemEvent?.Invoke((EquipableItem)target.Item);
    //        return true;
    //    }
    //    return false;
    //}
    public Item AddItem(EquipableItem item, int amount, out List<Item> PreviousItems)
    {
        EquipmentSlot shieldSlot = EquipmentSlots.FirstOrDefault(s => s.EquipmentType == EquipmentType.Shield);
        EquipmentSlot MainHandSlot = EquipmentSlots.FirstOrDefault(s => s.EquipmentType == EquipmentType.MainHand);
        PreviousItems = new List<Item>();
        Item MainPreviousItem = null;
        EquipmentSlot MainTarget;
        List<EquipmentSlot> targets = new List<EquipmentSlot>();
        if (item as Weapon != null)
        {
            switch (((Weapon)item).weaponType)
            {
                case Weapon.WeaponType.MainHand:
                    MainTarget = MainHandSlot;
                    if (shieldSlot.Item as Weapon != null && ((Weapon)shieldSlot.Item).weaponType == Weapon.WeaponType.Bow)
                    {
                        targets.Add(shieldSlot);
                        PreviousItems.Add(shieldSlot.Item);
                    }
                    break;
                case Weapon.WeaponType.OffHand:
                    MainTarget = shieldSlot;
                    if (MainHandSlot.Item != null)
                        if (((Weapon)MainHandSlot.Item).weaponType == Weapon.WeaponType.TwoHand
                        || ((Weapon)MainHandSlot.Item).weaponType == Weapon.WeaponType.Staff)
                        {
                            targets.Add(MainHandSlot);
                            PreviousItems.Add(MainHandSlot.Item);
                        }
                    break;
                case Weapon.WeaponType.TwoHand:
                    MainTarget = MainHandSlot;
                    if (shieldSlot.Item != null)
                    {
                        targets.Add(shieldSlot);
                        PreviousItems.Add(shieldSlot.Item);
                    }
                    break;
                case Weapon.WeaponType.Bow:
                    MainTarget = shieldSlot;
                    if (MainHandSlot.Item != null)
                    {
                        targets.Add(MainHandSlot);
                        PreviousItems.Add(MainHandSlot.Item);
                    }
                    break;
                case Weapon.WeaponType.Staff:
                    MainTarget = MainHandSlot;
                    if (shieldSlot.Item != null)
                    {
                        targets.Add(shieldSlot);
                        PreviousItems.Add(shieldSlot.Item);
                    }
                    break;
                default:
                    MainTarget = EquipmentSlots.FirstOrDefault(s => s.EquipmentType == item.EquipmentType);
                    break;
            }
        }
        else
        {
            MainTarget = EquipmentSlots.FirstOrDefault(s => s.EquipmentType == item.EquipmentType);
        }
        if (MainTarget != null)
        {
            MainPreviousItem = MainTarget.Item;
            RemoveItem(MainTarget);
            MainTarget.Item = item;
            MainTarget.Amount = amount;

        }
        RemoveItem(targets);
        OnAddItemEvent?.Invoke((EquipableItem)MainTarget.Item);
        return MainPreviousItem;
    }
    public bool AddItem(ItemSlot item, out List<Item> PreviousItems)
    {
        PreviousItems = new List<Item>();
        for (int i = 0; i < EquipmentSlots.Length; i++)
        {
            if (EquipmentSlots[i].CanRecieveItem(item.Item))
            {
                PreviousItems.Add((EquipableItem)EquipmentSlots[i].Item);
                RemoveItem(PreviousItems);
                EquipmentSlots[i].Item = item.Item;
                EquipmentSlots[i].Amount = item.Amount;
                OnAddItemEvent?.Invoke((EquipableItem)EquipmentSlots[i].Item);
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(List<Item> items)
    {
        if (items.Count > 0)
            foreach (var item in items)
            {
                if (item != null)
                {
                    OnRemoveItemEvent?.Invoke((EquipableItem)item);
                }
            }
        items = null;
    }
    public void RemoveItem(List<EquipmentSlot> slots)
    {
        foreach (var slot in slots)
        {
            if (slot.Item != null)
            {
                OnRemoveItemEvent?.Invoke((EquipableItem)slot.Item);
            }
            slot.Item = null;
            slot.Amount = 0;
        }
    }
    public void RemoveItem(EquipmentSlot item)
    {
        if (item.Item != null)
            OnRemoveItemEvent?.Invoke((EquipableItem)item.Item);
        item = null;
    }

    public bool RemoveItem(EquipableItem item, out List<Item> PreItems)
    {
        PreItems = null;
        var target = EquipmentSlots.FirstOrDefault(s => s.EquipmentType == item.EquipmentType);
        if (target.Item != null)
        {
            OnRemoveItemEvent?.Invoke(item);
            target.Amount = 0;
            target.Item = null;
            return true;
        }
        //for (int i = 0; i < EquipmentSlots.Length; i++)
        //{
        //    if (EquipmentSlots[i].Item == item)
        //    {
        //        //                CharacterScript.Instance.characterState.Remove(item.state);
        //        //PlayerVisualScript.Instance.RemoveItemVisual(item);
        //        OnRemoveItemEvent?.Invoke(item);
        //        EquipmentSlots[i].Item = null;
        //        return true;
        //    }
        //}

        return false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDropVoidEvent?.Invoke();
    }
    public EquipmentSlot GetSlot(EquipmentType type)
    {
        return EquipmentSlots.FirstOrDefault(s => s.EquipmentType == type);
    }
}