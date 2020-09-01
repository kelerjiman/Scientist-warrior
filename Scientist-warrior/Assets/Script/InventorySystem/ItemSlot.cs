using System;
using Script;
using Script.QuestSystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IDropHandler
{
    [SerializeField] Image image;
    [SerializeField] protected Text amountText;
    public event Action<ItemSlot> OnRightClickEvent;
    public event Action<ItemSlot> OnBeginDragEvent;
    public event Action<ItemSlot> OnDragEvent;
    public event Action<ItemSlot> OnEndDragEvent;
    public event Action<ItemSlot> OnDropEvent;
    public event Action<Item> ItemQuestEvent;

    [SerializeField] private Item m_Item;

    public Item Item
    {
        get { return m_Item; }
        set
        {
            m_Item = value;
            if (m_Item == null)
            {
                image.color = m_DisableColor;
                amountText.enabled = false;
            }
            else
            {
                image.color = m_NormalColor;
                image.sprite = m_Item.Icon;
                ItemQuestEvent?.Invoke(m_Item);
            }
        }
    }

    [Range(1, 999)] [SerializeField] protected int m_Amount;

    public virtual int Amount
    {
        get { return m_Amount; }
        set
        {
            m_Amount = value;
            if (m_Amount < 0)
                Amount = 0;
            if (m_Amount == 0)
                Item = null;
            amountText.enabled = m_Item != null && m_Amount > 1;
            if (amountText.enabled) amountText.text = m_Amount.ToString();
        }
    }


    private readonly Color m_NormalColor = Color.white;
    private readonly Color m_DisableColor = new Color(1, 1, 1, 0);

    protected virtual void OnValidate()
    {
        if (image == null)
            image = GetComponent<Image>();
        if (amountText == null)
            amountText = GetComponentInChildren<Text>();
    }

    public bool CanStackItem(Item item, int amount = 1)
    {
        if (Item == null)
            return false;
        return Item.Id == item.Id && Amount + amount <= Item.MaxStack;
    }

    public virtual bool CanRecieveItem(Item item)
    {
        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClickEvent?.Invoke(this);
        }
    }

    private Vector2 m_OriginalPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragEvent?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDropEvent?.Invoke(this);
    }
}