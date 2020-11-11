using Script.CharacterStatus;
using Script.ShopSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Script.InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] public CanvasRenderer ItemChestUI;
        [SerializeField] public Inventory inventory;
        [SerializeField] public EquipmentPanel equipmentPanel;
        [SerializeField] private BaseCraftingWindow craftingWindow;
        [SerializeField] private Image DragableItem;
        [SerializeField] private ItemDestroyPanel m_ItemDestroyPanel;
        private ItemSlot m_DraggedSlot;
        private ItemSlot m_DropedItemSlot;
        public static InventoryManager Instance;
        private Vector3 DefItemPosition = Vector2.zero;

        private void Awake()
        {
            //set up Event:
            inventory.ItemQuestEvent += InventoryOnItemQuestEvent;
            //On right Click Event
            inventory.OnRightClickEvent += Onclick;
            equipmentPanel.OnRightClickEvent += Onclick;
            //on Pointer Event Must be like that
            //            m_Inventory.OnPointerEnter += ShowTooltip;
            //            m_EquipmentPanel.OnPointerEnter += ShowToolTip;
            //            m_Inventory.OnPointerExit += HideTooltip;
            //            m_EquipmentPanel.OnPointerExit += HideToolTip;
            //On Begin Drag Event 
            inventory.OnBeginDragEvent += BeginDrag;
            equipmentPanel.OnBeginDragEvent += BeginDrag;
            //On End Drag Event
            inventory.OnEndDragEvent += EndDrag;
            equipmentPanel.OnEndDragEvent += EndDrag;
            //On  Drag Event
            inventory.OnDragEvent += Drag;
            equipmentPanel.OnDragEvent += Drag;
            //On Drop Event
            inventory.OnDropEvent += Drop;
            inventory.OnDropVoidEvent += Drop;
            equipmentPanel.OnDropEvent += Drop;
            equipmentPanel.OnDropVoidEvent += Drop;
            //        m_DropItemArea.OnDropEvent += DropItemOutSide;
            //Crafting
            craftingWindow.CraftingRecipeHandler += Craft;
            Instance = this;
        }

        private void InventoryOnItemQuestEvent(Item item)
        {
        }

        private void Start()
        {
            ItemChestUI.gameObject.SetActive(false);
        }


        private void BeginDrag(ItemSlot itemSlot)
        {
            if (itemSlot.Item != null)
            {
                DefItemPosition = Input.mousePosition;
                m_DraggedSlot = itemSlot;
                DragableItem.sprite = itemSlot.Item.Icon;
                DragableItem.GetComponent<CanvasGroup>().interactable = false;
                DragableItem.transform.position = Input.mousePosition;
                DragableItem.enabled = true;
            }
        }

        private void Drag(ItemSlot itemSlot)
        {
            if (DragableItem.enabled)
                DragableItem.transform.position = Input.mousePosition;
        }

        private void EndDrag(ItemSlot itemSlot)
        {
            if (m_DropedItemSlot == null)
                DropingOutSideUi(itemSlot);
            m_DraggedSlot = null;
            DragableItem.enabled = false;
            m_DropedItemSlot = null;
        }

        private void DropingOutSideUi(ItemSlot itemSlot)
        {
            if (m_DraggedSlot == null)
                return;
            m_ItemDestroyPanel.Show();
            ItemSlot targetItemSlot = m_DraggedSlot;
            m_ItemDestroyPanel.ItemIcon.sprite = targetItemSlot.Item.Icon;
            m_ItemDestroyPanel.OnYesButtonEvent += () =>
            {
                if (itemSlot as EquipmentSlot != null)
                    StateManager.Instance.RemoveState(((EquipableItem)targetItemSlot.Item).state);
                //            targetItemSlot.Item.Destroy();
                targetItemSlot.Item = null;
                m_ItemDestroyPanel.Hide();
            };
            m_ItemDestroyPanel.OnNoButtonEvent += () => { m_ItemDestroyPanel.Hide(); };
        }

        private void Drop(ItemSlot dropItemSlot)
        {
            m_DropedItemSlot = dropItemSlot;
            if (m_DraggedSlot == null)
                return;
            if (dropItemSlot.CanStackItem(m_DraggedSlot.Item))
            {
                AddStacks(dropItemSlot);
            }
            else if (dropItemSlot.CanRecieveItem(m_DraggedSlot.Item) && m_DraggedSlot.CanRecieveItem(dropItemSlot.Item))
            {
                if (dropItemSlot as EquipmentSlot != null)
                {
                    if (dropItemSlot.Item != null)
                    {
                        StateManager.Instance.RemoveState(((EquipableItem)dropItemSlot.Item).state);
                        PlayerVisualScript.Instance.RemoveItemVisual((EquipableItem)dropItemSlot.Item);
                    }

                    if (m_DraggedSlot.Item != null)
                    {
                        StateManager.Instance.AddState(((EquipableItem)m_DraggedSlot.Item).state);
                        PlayerVisualScript.Instance.SetItemVisual((EquipableItem)m_DraggedSlot.Item);
                    }
                }
                else
                {
                    if (m_DraggedSlot as EquipmentSlot != null)
                    {
                        if (dropItemSlot.Item != null)
                        {
                            StateManager.Instance.AddState(((EquipableItem)dropItemSlot.Item).state);
                            PlayerVisualScript.Instance.SetItemVisual((EquipableItem)dropItemSlot.Item);
                        }

                        if (m_DraggedSlot.Item != null)
                        {
                            StateManager.Instance.RemoveState(((EquipableItem)m_DraggedSlot.Item).state);
                            PlayerVisualScript.Instance.RemoveItemVisual((EquipableItem)m_DraggedSlot.Item);
                        }
                    }
                }

                SwapItem(dropItemSlot);
            }
        }

        private void Drop()
        {
            m_DropedItemSlot = m_DraggedSlot;
        }

        private void SwapItem(ItemSlot dropItemSlot)
        {
            Item DraggedItem = m_DraggedSlot.Item;
            int DraggedItemAmount = m_DraggedSlot.Amount;
            m_DraggedSlot.Item = dropItemSlot.Item;
            m_DraggedSlot.Amount = dropItemSlot.Amount;
            dropItemSlot.Item = DraggedItem;
            dropItemSlot.Amount = DraggedItemAmount;
        }

        private void AddStacks(ItemSlot dropItemSlot)
        {
            int RemainStacks = dropItemSlot.Item.MaxStack - dropItemSlot.Amount;
            int NumOfAddedStack = Mathf.Min(RemainStacks, m_DraggedSlot.Amount);
            dropItemSlot.Amount += NumOfAddedStack;
            m_DraggedSlot.Amount -= NumOfAddedStack;
        }

        private void Onclick(ItemSlot itemSlot)
        {
            if (ShopManager.Instance.SellWindowActive)
                return;
            //for Unequip item slot
            if (itemSlot as EquipmentSlot && itemSlot.Item != null)
            {
                if (!inventory.IsFull())
                {
                    inventory.AddItem(itemSlot.Item, itemSlot.Amount);
                    inventory.AddItemEvent?.Invoke(itemSlot.Item);

                    StateManager.Instance.RemoveState(((EquipableItem)itemSlot.Item).state);
                    PlayerVisualScript.Instance.RemoveItemVisual((EquipableItem)itemSlot.Item);
                    equipmentPanel.RemoveItem(itemSlot.Item as EquipableItem);
                    itemSlot.Item = null;
                }
            }
            //for Equip Item Slot
            else if (itemSlot.Item as EquipableItem != null && itemSlot.Item != null)
            {
                EquipableItem previosItem;
                int amount;
                equipmentPanel.AddItem(itemSlot, out previosItem, out amount);

                if (previosItem != null)
                {
                    StateManager.Instance.RemoveState(previosItem.state);
                    PlayerVisualScript.Instance.RemoveItemVisual(previosItem);
                }

                if (itemSlot != null)
                {
                    StateManager.Instance.AddState(((EquipableItem)itemSlot.Item).state);
                    PlayerVisualScript.Instance.SetItemVisual((EquipableItem)itemSlot.Item);
                }

                itemSlot.Item = previosItem;
                itemSlot.Amount = amount;
            }
            //for useable Item
            else if (itemSlot.Item as UseableItem)
            {
                if (itemSlot.Item.UseItem())
                    inventory.RemoveItem(itemSlot.Item,1);
            }
        }

        private void Craft(CraftingRecipe craftingRecipe)
        {
            if (!inventory.IsFull())
            {
                if (craftingRecipe.CanCraft(inventory))
                    craftingRecipe.Craft(inventory);
                else
                    Debug.Log("Required Items Not Reachable");
            }
            else
            {
                Debug.Log("inventory is full");
            }
        }
    }
}